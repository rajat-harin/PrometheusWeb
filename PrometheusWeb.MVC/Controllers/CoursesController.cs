using Newtonsoft.Json;
using PrometheusWeb.Data.DataModels;
using PrometheusWeb.Data.UserModels;
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
    public class CoursesController : Controller
    {
        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44375/";
        // GET: Course

        public ActionResult Index()
        {
            return View();
        }


        // GET: Course/ViewCourses
        public async Task<ActionResult> ViewCourses()
        {
            List<CourseUserModel> courses = new List<CourseUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
                HttpResponseMessage ResFromCourses = await client.GetAsync("api/Courses/");


                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromCourses.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var courseResponse = ResFromCourses.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the list  
                    courses = JsonConvert.DeserializeObject<List<CourseUserModel>>(courseResponse);

                }
                //returning the employee list to view  
                return View(courses);
            }
        }

    }
}