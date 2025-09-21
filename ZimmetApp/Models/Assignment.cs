using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZimmetApp.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        [Required]
        public int PersonId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public Person? Person { get; set; }

        [Required]
        public ItemType ItemType { get; set; }

        [Required, StringLength(150)]
        public string ItemName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SerialOrLine { get; set; }

        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; } = DateTime.Today;

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
