using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrometheusWeb.MVC.Models.ViewModels
{
    public class UserViewModel
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
    }
}