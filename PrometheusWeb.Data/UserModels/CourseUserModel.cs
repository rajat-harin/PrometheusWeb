using System;

namespace PrometheusWeb.Data.UserModels
{
    public class CourseUserModel
    {
        public int CourseID { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
