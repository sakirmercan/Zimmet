using System.ComponentModel.DataAnnotations;

namespace ZimmetApp.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Department { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }
    }
}
