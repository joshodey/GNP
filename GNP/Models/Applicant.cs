using System.ComponentModel.DataAnnotations.Schema;

namespace GNP.Models
{
    public class Applicant : BaseEntity<long>
    {
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }
        public User User { get; set; }
        public ApplicationForm ApplicationForm { get; set; }
    }
}
