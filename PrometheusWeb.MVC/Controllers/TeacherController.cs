using Newtonsoft.Json;
using PrometheusWeb.Data.UserModels;
using PrometheusWeb.MVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PrometheusWeb.MVC.Controllers
{
    public class TeacherController : Controller
    {
        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44375/";
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        // GET: Teacher/MyCourses
        public async Task<ActionResult> MyCourses(int TeacherId = 1)  //@TODO: change default to 0 after auth
        {
            List<CourseUserModel> courses = new List<CourseUserModel>();
            List<TeacherCourseUserModel> teachingCourses = new List<TeacherCourseUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Teacher Courses using HttpClient  
                HttpResponseMessage ResFromCourses = await client.GetAsync("api/Courses/");
                HttpResponseMessage ResFromTeachingCourses = await client.GetAsync("api/Teaches/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromCourses.IsSuccessStatusCode && ResFromTeachingCourses.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var courseResponse = ResFromCourses.Content.ReadAsStringAsync().Result;
                    var TeachingCourseResponse = ResFromTeachingCourses.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    courses = JsonConvert.DeserializeObject<List<CourseUserModel>>(courseResponse);
                    teachingCourses = JsonConvert.DeserializeObject<List<TeacherCourseUserModel>>(TeachingCourseResponse);

                    try
                    {
                        var result = teachingCourses.Where(item => item.TeacherID == TeacherId).Join(
                        courses,
                        teachingCourse => teachingCourse.CourseID,
                        course => course.CourseID,
                        (teachingCourse, course) => new TeacherCourses
                        {
                            TeacherID = (int)teachingCourse.TeacherID,
                            CourseID = (int)teachingCourse.CourseID,
                            Name = course.Name,
                            StartDate = (DateTime) course.StartDate,
                            EndDate = (DateTime) course.EndDate
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
                //returning the Courses list to view  
                return new HttpStatusCodeResult(404);
            }
        }

        // BELOW TWO TO BE MOVED TO COURSE CONTROLLER
        // GET: Teacher/ViewCourses
        public async Task<ActionResult> ViewCourses()  //@TODO: Id to be changed default to 0 after auth
        {
            List<CourseUserModel> courses = new List<CourseUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource using HTTP Client
                HttpResponseMessage ResFromCourses = await client.GetAsync("api/Courses/");


                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromCourses.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var courseResponse = ResFromCourses.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the list  
                    courses = JsonConvert.DeserializeObject<List<CourseUserModel>>(courseResponse);

                }
                //returning the Courses list to view  
                return View(courses);
            }
        }
        public async Task<ActionResult> SaveCourses(int courseId, int teacherId)
        {
            var client = new HttpClient();
            //Passing service base url  
            client.BaseAddress = new Uri(Baseurl);



            client.DefaultRequestHeaders.Clear();
            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



            //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
            HttpResponseMessage ResFromCourses = await client.GetAsync("api/Courses/" + courseId.ToString());
            HttpResponseMessage ResFromTeaches = await client.GetAsync("api/Teaches/" + teacherId.ToString());

            //Storing the response details recieved from web api   
            var teacherCourseResponse = ResFromCourses.Content.ReadAsAsync<CourseUserModel>().Result;



            return View(teacherCourseResponse);
        }


        //POST : Teacher/SaveCourses
        [HttpPost]
        public async Task<ActionResult> SaveCourses(CourseUserModel courseModel)
        {
            int TeacherID = 1;
            if (courseModel.StartDate.HasValue)
            {
                TimeSpan diff = DateTime.Now - (DateTime)courseModel.StartDate;
                if (diff.Days > 7)
                {
                    TempData["ErrorMessage"] = "The Course cannot be Selected!";
                    ViewBag.Message = "The Course cannot be Selected!";
                    return View();
                }
            }
            TeacherCourseUserModel teaches = new TeacherCourseUserModel
            {
                CourseID = courseModel.CourseID,
                TeacherID = TeacherID
            };
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);



                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                //Sending request to Post web api REST service resource using HttpClient  
                HttpResponseMessage ResFromTeaches = await client.PostAsJsonAsync("api/Teaches/", teaches);



                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromTeaches.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Selected Successfully";
                }
                else if (ResFromTeaches.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ErrorMessage"] = "Already Selected!";
                }
                else
                {
                    TempData["ErrorMessage"] = "There was error Selecting Course!";
                }
            }
            return RedirectToAction("MyCourses");
        }

        //View My Students
        public async Task<ActionResult> MyStudents(int courseId, int teacherId = 1)  //@TODO: change default to 0 after auth
        {
            List<StudentUserModel> students = new List<StudentUserModel>();
            List<EnrollmentUserModel> enrollments = new List<EnrollmentUserModel>();
            List<CourseUserModel> courses = new List<CourseUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage ResFromStudents = await client.GetAsync("api/Students/");
                HttpResponseMessage ResFromEnrollment = await client.GetAsync("api/Enrollments/");
                
                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromStudents.IsSuccessStatusCode && ResFromEnrollment.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var studentResponse = ResFromStudents.Content.ReadAsStringAsync().Result;
                    var enrollmentResponse = ResFromEnrollment.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    students = JsonConvert.DeserializeObject<List<StudentUserModel>>(studentResponse);
                    enrollments = JsonConvert.DeserializeObject<List<EnrollmentUserModel>>(enrollmentResponse);

                    try
                    {
                        var result = enrollments.Where(item => item.CourseID == courseId).Join(
                        students,
                        enrollment => enrollment.StudentID,
                        student => student.StudentID,
                        (enrollment, student) => new StudentUserModel
                        {
                            StudentID = student.StudentID,
                            FName = student.FName,
                            LName = student.LName,
                            MobileNo = student.MobileNo,
                            Address = student.Address,
                            City = student.City,
                            DOB = student.DOB
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

        // POST: Admin/UpdateTeacher
        public ActionResult UpdateTeacher(int id = 1)
        {
            if (id != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Teachers/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<TeacherUserModel>().Result);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateTeacher(TeacherUserModel teacher)
        {
            if (teacher.TeacherID != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Teachers/" + teacher.TeacherID, teacher).Result;
                TempData["SuccessMessage"] = "Teacher Updated Successfully";
            }
            return RedirectToAction("ViewTeachers");
        }
    }
}