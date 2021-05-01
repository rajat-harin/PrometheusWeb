using System;

namespace PrometheusWeb.Data.UserModels
{
    public class StudentUserModel
    {
        public int StudentID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string UserID { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
        public string City { get; set; }
        public string MobileNo { get; set; }
    }
}
