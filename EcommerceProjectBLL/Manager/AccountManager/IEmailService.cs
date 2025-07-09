using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Manager.AccountManager
{
    public interface IEmailService
    {
        public Task SendVerificationEmail(string email, string verificationCode);
        public Task SendPasswordResetEmail(string email, string resetToken);
    }
}
