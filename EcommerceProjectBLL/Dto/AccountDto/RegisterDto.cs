using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.AccountDto
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Email is required"),DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="Invalid Email format")]
        public string Email { get; set; }

        [Required,DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required"), DataType(DataType.Password)]
        //0=>name , 1=>max , 0=> min
        [StringLength(30,MinimumLength = 5,ErrorMessage = "{0} must be between {2} and {1} characters ")]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
