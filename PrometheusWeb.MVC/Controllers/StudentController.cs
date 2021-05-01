using Newtonsoft.Json;
using PrometheusWeb.Data;
using PrometheusWeb.Data.DataModels;
using PrometheusWeb.Data.UserModels;
using PrometheusWeb.MVC.Models.ViewModels;
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

        // GET: Student/MyCourses
        public async Task<ActionResult> MyCourses(int id = 1)  //@TODO: change default to 0 after auth
        {
            List<CourseUserModel> courses = new List<CourseUserModel>();
            List<EnrollmentUserModel> enrollments = new List<EnrollmentUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
                HttpResponseMessage ResFromCourses = await client.GetAsync("api/Courses/");
                HttpResponseMessage ResFromEnrollment = await client.GetAsync("api/Enrollments/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromCourses.IsSuccessStatusCode && ResFromEnrollment.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var courseResponse = ResFromCourses.Content.ReadAsStringAsync().Result;
                    var enrollmentResponse = ResFromEnrollment.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    courses = JsonConvert.DeserializeObject<List<CourseUserModel>>(courseResponse);
                    enrollments = JsonConvert.DeserializeObject<List<EnrollmentUserModel>>(enrollmentResponse);

                    try
                    {
                        var result = enrollments.Where(item => item.StudentID == id).Join(
                    courses,
                    enrollment => enrollment.CourseID,
                    course => course.CourseID,
                    (enrollment, course) => new EnrolledCourse
                    {
                        EnrollmentID = enrollment.EnrollmentID,
                        CourseID = (int)enrollment.CourseID,
                        Name = course.Name,
                        StartDate = course.StartDate,
                        EndDate = course.EndDate
                    }
                    ).ToList();
                        if (result.Any())
                        {
                            return View(result);
                        }
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(500);
                    }

                }
                //returning the employee list to view  
                return new HttpStatusCodeResult(404);
            }
        }

        // GET: Student/ViewCourses
        public async Task<ActionResult> ViewCourses(int id = 1)  //@TODO: change default to 0 after auth
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