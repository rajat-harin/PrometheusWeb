using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrometheusWeb.Data.UserModels
{
    [Table("Course")]
    public class CourseUserModel
    {
        public int CourseID { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
