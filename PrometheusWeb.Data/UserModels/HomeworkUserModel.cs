using System;
using System.ComponentModel.DataAnnotations;

namespace PrometheusWeb.Data.UserModels
{
    public class HomeworkUserModel
    {
        public int HomeWorkID { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "The Description can be upto 100 Words.", MinimumLength = 10)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "DeadLine")]
        public DateTime? Deadline { get; set; }

        [Required]
        [Display(Name = "RequiredDate")]
        public DateTime? ReqTime { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The Long Description can be upto 300 Words.", MinimumLength = 10)]
        [Display(Name = "LongDescription")]
        public string LongDescription { get; set; }
    }
}
