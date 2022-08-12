using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacementApplicationNew.Model
{
    public class Student
    {
        [Key]
        [Display(Name = "User Id")]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }       
        [Required]
        [Display(Name="Last Name")]
        public string? LastName { get; set; }
        [Required]
        [Display(Name = "Father Name")]
        public string? FatherName { get; set; }
        [Required]
        [Display(Name = "Branch Name")]
        public string? BranchName { get; set; }
        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Graduation Year")]
        public DateTime GraduationYear { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Please enter a range between 0 and 100")]
        [Display(Name = "Class 10th Percentage")]
        public int Class10thMarks { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Please enter a range between 0 and 100")]
        [Display(Name = "Class 12th Percentage")]
        public int Class12thMarks { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "Please enter a range between 0 and 10")]
        [Display(Name = "CGPA upto last sem")]
        public float CurrentCgpa { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{6,}$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [NotMapped]
        [Display(Name = "Confirm Password")]
        public string? CPassword { get; set; }
        public virtual ICollection<Apply>? Applys { get; set; }
    }
}
