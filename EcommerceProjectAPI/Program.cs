
using EcommerceProjectBLL.Manager.AccountManager;
using EcommerceProjectBLL.Manager.CategoryManager;
using EcommerceProjectDAL.Data.DbHelper;
using EcommerceProjectDAL.Data.Models;
using EcommerceProjectBLL.AutoMapper.CategoryAutoMapper;
using EcommerceProjectDAL.Repository.CategoryRepo;
using EcommerceProjectDAL.Repository.OrderItem;
using EcommerceProjectDAL.Repository.OrderItemRepo;
using EcommerceProjectDAL.Repository.OrderRepo;
using EcommerceProjectDAL.Repository.PaymentRepo;
using EcommerceProjectDAL.Repository.ProductRepo;
using EcommerceProjectDAL.Repository.ReviewRepo;
using EcommerceProjectDAL.Repository.ShoppingCartItemRepo;
using EcommerceProjectDAL.Repository.ShoppingCartRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EcommerceProjectBLL.Manager.ProductManager;
using EcommerceProjectBLL.AutoMapper.ProductAutoMapper;
using EcommerceProjectBLL.AutoMapper.OrderAutoMapper;
using EcommerceProjectBLL.Manager.OrderManager;
using EcommerceProjectBLL.AutoMapper.OrderItemAutoMapper;
using EcommerceProjectBLL.Manager.OrderItemManager;
using EcommerceProjectBLL.AutoMapper.ShoppingCartMapper;
using EcommerceProjectBLL.Manager.ShoppingCartManger;
using EcommerceProjectBLL.Manager.ShoppingCartManager;
using EcommerceProjectBLL.AutoMapper.ShoppingCartItemMapper;
using EcommerceProjectBLL.Manager.ShoppingCartItemManager;
using NETCore.MailKit.Core;
using NETCore.MailKit.Extensions;
using EcommerceProjectAPI.Configurations;
using Microsoft.Extensions.Configuration;
using Stripe;
using EcommerceProjectBLL.Manager.PaymentManager;
using EcommerceProjectBLL.AutoMapper.ReviewAutoMapper;
using EcommerceProjectBLL.Manager.ReviewManager;
using EcommerceProjectBLL.SeedingData;
using EcommerceProjectDAL.SeedData;
using Microsoft.OpenApi.Models;

namespace EcommerceProjectAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                        Enter 'Bearer' [space] and then your token.
                        Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                            },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                    },
                    new List<string>()
                    }
               });
            });


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequireUppercase=true;
            }).AddEntityFrameworkStores<EcommerceProjectContext>().AddDefaultTokenProviders();
            /////////////////////
           
            //
            RegisterServices(builder.Services);
            //[authorize]
            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme="JWT";//make sure token is true
                Options.DefaultChallengeScheme="JWT";//return 401 => unauthorized or 403 => forbeden
            }).AddJwtBearer("JWT", Options =>
            {
                //secrete key
                var SecretKeyString = builder.Configuration.GetValue<string>("SecratKey");
                var SecreteKeyBytes = Encoding.ASCII.GetBytes(SecretKeyString);
                SecurityKey securityKey = new SymmetricSecurityKey(SecreteKeyBytes);
                //--------------------------------------------------------------

                Options.TokenValidationParameters=new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    IssuerSigningKey=securityKey,
                    //false mean anyone can send and eny one can take
                    ValidateIssuer=false,//take token(backend)//make token
                    ValidateAudience=false//send token(frontend)//use token
                };
            });
            var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();

            builder.Services.AddMemoryCache();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<EcommerceProjectBLL.Manager.AccountManager.IEmailService, EcommerceProjectBLL.Manager.AccountManager.EmailService>();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            builder.Services.AddScoped<IPaymentManger, PaymentManger>();
           

            builder.Services.AddDbContext<EcommerceProjectContext>(options =>
                      options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            builder.Services.AddScoped<IAccountManager, AccountManager>();
            
            builder.Services.AddAutoMapper(map => map.AddProfile(new ProductMappingProfile()));
            builder.Services.AddScoped<IProductManger, ProductManger>();

            builder.Services.AddAutoMapper(map => map.AddProfile(new CategoryMappingProfile()));
            builder.Services.AddScoped<ICategoryManager, CategoryManager>();

            builder.Services.AddAutoMapper(map => map.AddProfile(new OrderMappingProfile()));
            builder.Services.AddScoped<IOrderManger, OrderManger>();

            builder.Services.AddAutoMapper(map => map.AddProfile(new ReviewMappingProfile()));
            builder.Services.AddScoped<IReviewManger, ReviewManger>();

            builder.Services.AddAutoMapper(map => map.AddProfile(new OrderItemMappingProfile()));
            builder.Services.AddScoped<IOrderItemManager,OrderItemManager>();

            builder.Services.AddAutoMapper(map => map.AddProfile(new ShoppingCartMappingProfile()));
            builder.Services.AddScoped<IShoppingCartManager, ShoppingCartManager>();

            builder.Services.AddAutoMapper(map => map.AddProfile(new ShoppingCartItemMappingProfile()));
            builder.Services.AddScoped<IShoppingCartItemManager, ShoppingCartItemManager>();


            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await IdentitySeedData.SeedRolesAsync(services);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedData.SeedAdminAsync(services);
            }

            app.Run();
        }
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IReviewRepo, ReviewRepo>();
            services.AddScoped<IShoppingCartItemRepo, ShoppingCartItemRepo>();
            services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();
        }
    }
}
