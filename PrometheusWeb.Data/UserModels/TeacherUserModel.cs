using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrometheusWeb.Data.UserModels
{
    [Table("Teacher")]
    public class TeacherUserModel
    {
        public int TeacherID { get; set; }

        
        public string FName { get; set; }

        
        public string LName { get; set; }

        
        public string UserID { get; set; }

        
        public string Address { get; set; }

        public DateTime? DOB { get; set; }

        
        public string City { get; set; }

        
        public string MobileNo { get; set; }

        public bool? IsAdmin { get; set; }
    }
}
