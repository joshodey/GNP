using System.ComponentModel.DataAnnotations;

namespace GNP.Controllers
{
    public class LoginSignUpVM
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Please input a valid Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool ConfirmPasswordReset 
        {
            get
            {
                return this.Password == this.ConfirmPassword;
            }   
        }
    }
}
