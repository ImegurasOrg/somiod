using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace somiod.Models
{
    public class Data{
		//data: id <int>; content <string>; creation_dt <string/ISO DateTime>; parent <int>
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Creation_DT { get; set; }

        [Required]
        public Module Parent { get; set; } //Id of the module application
    }
}
