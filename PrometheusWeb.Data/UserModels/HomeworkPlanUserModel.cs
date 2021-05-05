using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrometheusWeb.Data.UserModels
{
    public class HomeworkPlanUserModel
    {
        public int? HomeworkPlanID { get; set; }

        public int? StudentID { get; set; }

        public int? HomeworkID { get; set; }

        public int? PriorityLevel { get; set; }

        public bool? isCompleted { get; set; }

        public HomeworkUserModel Homework { get; set; }
    }
}
