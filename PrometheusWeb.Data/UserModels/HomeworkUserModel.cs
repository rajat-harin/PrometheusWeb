using System;

namespace PrometheusWeb.Data.UserModels
{
    public class HomeworkUserModel
    {
        public int HomeWorkID { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? ReqTime { get; set; }
        public string LongDescription { get; set; }
    }
}
