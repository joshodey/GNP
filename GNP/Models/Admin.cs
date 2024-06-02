using System.ComponentModel.DataAnnotations.Schema;

namespace GNP.Models
{
    public enum AdminType
    {
        Admin1,
        Admin2
    }
    public class Admin : BaseEntity<long>
    {
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }
        public User User { get; set; }
        public List<ApplicationForm> Form { get; set; }
        public AdminType AdminType { get; set; }
    }
}
