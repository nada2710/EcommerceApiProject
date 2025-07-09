using EcommerceProjectBLL.Dto.AccountDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.AccountManager
{
    public interface IAccountManager
    {
        Task<AuthModel> Login(LoginDto loginDto);
        Task<AuthModel> Register(RegisterDto registerDto);
        Task<AuthModel> VerifyEmail(VerifyEmailDto verifyEmailDto);
        Task<AuthModel> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<AuthModel> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
