using System;
using System.ComponentModel.DataAnnotations;

namespace PrometheusWeb.Data.UserModels
{
    public class HomeworkUserModel
    {
        public int HomeWorkID { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "DeadLine")]
        public DateTime? Deadline { get; set; }

        [Required]
        [Display(Name = "RequiredDate")]
        public DateTime? ReqTime { get; set; }

        [Required]
        [Display(Name = "LongDescription")]
        public string LongDescription { get; set; }
    }
}
