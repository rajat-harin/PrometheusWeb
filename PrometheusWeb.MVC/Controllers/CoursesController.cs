using Newtonsoft.Json;
using PrometheusWeb.Data.DataModels;
using PrometheusWeb.Data.UserModels;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PrometheusWeb.MVC.Controllers
{
    public class CoursesController : Controller
    {
        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44375/";
        // GET: Course

        public async Task<ActionResult> Index()
        {
            List<Course> CourseInfo = new List<Course>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllCourses using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Courses/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var CouResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    CourseInfo = JsonConvert.DeserializeObject<List<Course>>(CouResponse);

                }
                //returning the employee list to view  
                return View(CourseInfo);
            }
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


        public ActionResult AddOrEditCourses(int id = 0)
        {
            if (id == 0)
                return View(new CourseUserModel());
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Courses/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<CourseUserModel>().Result);
            }
        }
        [HttpPost]
        public ActionResult AddOrEditCourses(CourseUserModel course)
        {
            if (course.CourseID == 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Courses/", course).Result;
                TempData["SuccessMessage"] = "Course Added Successfully";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Courses/" + course.CourseID, course).Result;
                TempData["SuccessMessage"] = "Course Updated Successfully";
            }
            return RedirectToAction("ViewCourses");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Courses/" + id.ToString()).Result;
            return RedirectToAction("ViewCourses");
        }

        /*
           [HttpPost]
           // GET: Course/AddCourses
           public async Task<ActionResult> AddCourses()
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
                   HttpResponseMessage ResFromCourses = await client.PostAsJsonAsync("api/Courses/", courses);


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
        */
        /*
                public ActionResult EditCourses(int id)
                {
                    return View();
                }

                [HttpPut]
                public ActionResult EditCourses(CourseUserModel course)
                {
                    if (course.CourseID != 0)
                    {
                        HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Courses/"+ course.CourseID, course).Result;
                        return RedirectToAction("ViewCourses");
                    }

                    return RedirectToAction("EditCourses");
                }
        */


        /*
           [HttpPut]
           // GET: Course/EditCourses
           public async Task<ActionResult> EditCourses()
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
                   HttpResponseMessage ResFromCourses = await client.PutAsJsonAsync("api/Courses/", courses);


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
        */
        /*
        [HttpDelete]
           // GET: Course/EditCourses
           public async Task<ActionResult> DeleteCourses()
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
                   HttpResponseMessage ResFromCourses = await client.DeleteAsync("api/Courses/");


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
        */

    }
}