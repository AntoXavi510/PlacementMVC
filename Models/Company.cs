using System.ComponentModel.DataAnnotations;

namespace PlacementApplicationNew.Model
{
    public class Company
    {
        [Key]
        [Display(Name = "Company Id")]
        public int CompanyId { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }
        public virtual ICollection<Role>? Roles { get; set; }
    }
}
