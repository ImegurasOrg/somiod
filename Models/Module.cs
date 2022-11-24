using System.ComponentModel.DataAnnotations;

namespace somiod.Models
{
    public class Module
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Creation_DT { get; set; }

        [Required]
        public Application Parent { get; set; } //Id of the module application
    }
}
