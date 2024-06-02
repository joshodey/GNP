using System.ComponentModel.DataAnnotations;

namespace GNP.Models
{
    public class BaseEntity<Tkey>
    {
        [Key]
        public Tkey Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class BaseEntity : BaseEntity<Guid> { }
}
