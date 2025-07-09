using EcommerceProjectBLL.Dto.AccountDto;
using EcommerceProjectDAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;



namespace EcommerceProjectBLL.Manager.AccountManager
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AccountManager> _logger;
        private const string PendingUsersCacheKey = "PendingUsers_";

        public AccountManager(UserManager<ApplicationUser> userManager,IConfiguration configuration, IEmailService emailService, IMemoryCache cache, ILogger<AccountManager> logger)
        {
            _userManager=userManager;
            _configuration=configuration;
            _emailService=emailService;
            _cache=cache;
            _logger = logger;
        }
        private string GenerateVerificationCode()
        {
            // Using cryptographically secure random number generator
            var randomNumber = new byte[4];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToHexString(randomNumber)[..6].ToUpper(); // 6-character code
        }
        public async Task<AuthModel> Register(RegisterDto registerDto)
        {
            var auth = new AuthModel();
            //1.make user email and name is not in the db
            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
            {
                auth.Message = "Email is already registered!";
                return auth;
            }
            if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
            {
                auth.Message ="Name is already registered!";
                return auth;
            }
            if(_cache.TryGetValue($"{PendingUsersCacheKey}{registerDto.Email}",out PendingUserData existingPending))
            {
                var timeSinceLastRequest = DateTime.UtcNow-(existingPending.Expiration.AddHours(-24));
                if(timeSinceLastRequest.TotalMinutes<5)
                {
                    auth.Message="Verification email already send , please wait 5 minutes before requesting another .";
                    return auth;
                }
                //if 5 minute have passed,allow sending a new code
                _cache.Remove($"{PendingUsersCacheKey}{registerDto.Email}");
            }
             // Create pending user record
            var pendingUser = new PendingUserData
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                Password = registerDto.Password,
                PhoneNumber = registerDto.PhoneNumber,
                VerificationCode = GenerateVerificationCode(),
                Expiration = DateTime.UtcNow.AddHours(24)
            };
             // Store in cache
            try
            {
                await _emailService.SendVerificationEmail(pendingUser.Email, pendingUser.VerificationCode);
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(pendingUser.Expiration)
                    .SetPriority(CacheItemPriority.Normal);

                _cache.Set($"{PendingUsersCacheKey}{pendingUser.Email}", pendingUser, cacheOptions);
                auth.Message = "Verification email sent";
                auth.IsAuthenticated=true;
                return auth;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send verification email}");
                auth.Message = "Failed to send verification email";
                return auth;
            }
        }
        public async Task<AuthModel> Login(LoginDto loginDto)
        {
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if(User==null||!await _userManager.CheckPasswordAsync(User, loginDto.Password))
            {
                return new AuthModel
                {
                    Message="Email or Password is incorrect"
                };
            }
            if (!User.IsActive)
            {
                return new AuthModel { Message = "Please verify your email before logging in!" };
            }
            var JwtSecurityToken = CreateJwtToken(User);
           
            return new AuthModel
            {
                Message=$"{User.UserName} login successfully\"",
                IsAuthenticated=true,
            };
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var UserClaims = await _userManager.GetClaimsAsync(user);
            var Roles = await _userManager.GetRolesAsync(user);
            var RoleClaims = new List<Claim>();
            foreach (var Rolename in Roles)
            {
                RoleClaims.Add(new Claim(ClaimTypes.Role, Rolename));
            }
            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)

            }.Union(UserClaims)
             .Union(RoleClaims);

            var SecretKeyString = _configuration.GetSection("SecratKey").Value;
            var SecreteKeyBytes = Encoding.ASCII.GetBytes(SecretKeyString);
            SecurityKey securityKey = new SymmetricSecurityKey(SecreteKeyBytes);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);//(secrete key + hash algorithm)
            //Expiredate
            var Expiredate = DateTime.Now.AddDays(20);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                claims: Claims,
                signingCredentials: signingCredentials,
                expires: Expiredate
                );
            return jwtSecurityToken;
        }
        public async Task<AuthModel> VerifyEmail(VerifyEmailDto verifyEmailDto)
        {
            // Get pending user from cache
            if (!_cache.TryGetValue($"{PendingUsersCacheKey}{verifyEmailDto.Email}", out PendingUserData pendingUser))
            {
                return new AuthModel { 
                     Message = "Invalid or expired verification request"
                    ,IsAuthenticated=false
                };
            }
            // Validate verification code
            if (pendingUser.VerificationCode != verifyEmailDto.VerificationCode)
            {
                return new AuthModel { Message = "Invalid verification code" ,IsAuthenticated=false};
            }
            // Create real user account
            var user = new Customer
            {
                Email = pendingUser.Email,
                UserName = pendingUser.UserName,
                PhoneNumber = pendingUser.PhoneNumber,
                VerificationCode=pendingUser.VerificationCode,
                EmailConfirmed = true,
                IsActive = true,
                UserRole=UserRole.Customer
            };
            var result = await _userManager.CreateAsync(user, pendingUser.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                _logger.LogError("User creation failed: {Errors}", string.Join(", ", errors));
                return new AuthModel { Message = "User creation failed", Errors = errors.ToList() ,IsAuthenticated=false};
            }
                // Clean up pending registration
                _cache.Remove($"{PendingUsersCacheKey}{verifyEmailDto.Email}");
            // Generate JWT token
            var token = await CreateJwtToken(user);
            return new AuthModel
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpairationDate = token.ValidTo,
                Message = "Registration completed successfully"
            };
        }

        public async Task<AuthModel> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var model = new AuthModel();
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

            if (user == null)
            {
                model.Message = "If the account exists, you will receive a password reset token.";
                return model;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendPasswordResetEmail(forgotPasswordDto.Email, token);
            model.Message = "If the email is correct, a password reset token has been sent.";
            return model;
        }

        public async Task<AuthModel> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var model = new AuthModel();
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if(user==null)
            {
                model.Message="Invalid request.";
                model.Errors=new List<string> { "User not found." };
                return model;
            }
            var result = await _userManager.ResetPasswordAsync(user,
             resetPasswordDto.Token,
             resetPasswordDto.NewPassword);
            if(result.Succeeded)
            {
                model.Message="Password has been reset successfully!";
                return model;
            }
            model.Message = "Password reset failed.";
            model.Errors = result.Errors.Select(e => e.Description).ToList();
            return model;
        }
    }
}
