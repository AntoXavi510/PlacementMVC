using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacementApplicationNew.Model
{
    public class Role
    {
        [Key]
        [Display(Name = "Role Id")]
        public int RoleId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string? RoleName { get; set; }
        [Required]
        [Display(Name = "Salary Package")]
        public long SalaryPackage { get; set; }
        [Required]

        public string? Location { get; set; }
        [Required]
        [Range(0, 10)]
        [Display(Name = "Required CGPA")]
        public int CutoffPercentage { get; set; }
        [Required]
        [Display(Name = "Date Of Drive")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfDrive { get; set; }
        public int? CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company? Company { get; set; }

        public virtual ICollection<Apply>? Applys { get; set; }
    }
}
