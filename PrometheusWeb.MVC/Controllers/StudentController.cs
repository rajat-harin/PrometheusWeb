using Newtonsoft.Json;
using PrometheusWeb.Data;
using PrometheusWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PrometheusWeb.MVC.Controllers
{
    public class StudentController : Controller
    {
        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44375/";
        // GET: Student
        [Authorize(Roles = "admin, teacher")]
        public async Task<ActionResult> Index()
        {
            List<Student> EmpInfo = new List<Student>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Students/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var StuResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<List<Student>>(StuResponse);

                }
                //returning the employee list to view  
                return View(EmpInfo);
            }
        }
    }
}