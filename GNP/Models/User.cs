using Microsoft.AspNetCore.Identity;

namespace GNP.Models
{
    public enum UserType
    {
        Admin,
        Applicant
    }

    public class User : IdentityUser<long>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserType UserType { get; set; }
      
    }
}
