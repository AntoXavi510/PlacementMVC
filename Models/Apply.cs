using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacementApplicationNew.Model
{
    public class Apply
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Student Id")]
        public int? StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }
        [Display(Name = "Role Id")]
        public int? RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }

    }
}
