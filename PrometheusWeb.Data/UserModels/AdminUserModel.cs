using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrometheusWeb.Data.UserModels
{
    public class AdminUserModel
    {
        [Display(Name = "Teacher ID")]
        public int TeacherID { get; set; }

        [Display(Name = "Student ID")]
        public int StudentID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required]
        [Display(Name = "UserID")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "Date of Birth (YYYY-MM-DD)")]
        public DateTime? DOB { get; set; }

        [Required]
        [Display(Name = "Residential Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The Phone Number should be 10 digits only.", MinimumLength = 10 )]
        [Display(Name = "Phone Number")]
        public string MobileNo { get; set; }

        [Display(Name = "Is Admin?")]
        public bool IsAdmin { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Required]
        [Display(Name = "SecurityQuestion")]
        public string SecurityQuestion { get; set; }

        [Required]
        [Display(Name = "SecurityAnswer")]
        public string SecurityAnswer { get; set; }
    }
}
