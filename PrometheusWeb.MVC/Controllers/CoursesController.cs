using Newtonsoft.Json;
using PrometheusWeb.Data.DataModels;
using PrometheusWeb.Data.UserModels;
using PrometheusWeb.MVC.Models.ViewModels;
using PrometheusWeb.Utilities;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public ActionResult Index()
        {
            ViewBag.Title = "Course Index Page";
            return View();
        }

        // POST: Course/AddOrEditCourses
        public ActionResult AddCourse(int id = 0)
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
        public ActionResult AddCourse(CourseUserModel course)
        {
            if (course.CourseID == 0)
            {
                if (course.StartDate.HasValue)
                {
                    TimeSpan diff = (DateTime)course.EndDate - (DateTime)course.StartDate;
                    if (diff.Days == 0)
                    {
                        TempData["ErrorMessage"] = "Course StartDate cannot be same with EndDate";
                        return View();
                    }
                }
                if (course.EndDate.HasValue)
                {
                    TimeSpan diff = (DateTime)course.EndDate - (DateTime)course.StartDate;
                    if (diff.Days < 0)
                    {
                        TempData["ErrorMessage"] = "Course EndDate cannot be before StartDate";
                        return View();
                    }
                }
                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Courses/", course).Result;
                if (responseStudent.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Student Added Successfully";
                    ViewBag.Message = "Student Added Successfully";

                    TempData["SuccessMessage"] = "Student Added Successfully";
                    ViewBag.Message = "Student Added Successfully";

                }
                else if (responseStudent.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ErrorMessage"] = "Phone No Already Taken try another Phone No";
                    ViewBag.Message = "Phone No Already Taken try another Phone No";
                }

                else
                {
                    TempData["ErrorMessage"] = "There was error registering a Teacher!";
                    ViewBag.Message = "There was error registering a Teacher!";
                }
            }
            else
            {
                if (course.StartDate.HasValue)
                {
                    TimeSpan diff = (DateTime)course.EndDate - (DateTime)course.StartDate;
                    if (diff.Days == 0)
                    {
                        TempData["ErrorMessage"] = "Course StartDate cannot be same with EndDate";
                        return View();
                    }
                }
                if (course.EndDate.HasValue)
                {
                    TimeSpan diff = (DateTime)course.EndDate - (DateTime)course.StartDate;
                    if (diff.Days < 0)
                    {
                        TempData["ErrorMessage"] = "Course EndDate cannot before/same as StartDate";
                        return View();
                    }
                }
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Courses/" + course.CourseID, course).Result;
                TempData["SuccessMessage"] = "Course Updated Successfully";
            }
            return RedirectToAction("ViewCourses");
        }

        // POST: Admin/EditTeacherProfile
        public ActionResult UpdateCourse(int id = 0)
        {
            if (id != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Courses/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<CourseUserModel>().Result);
            }
            return RedirectToAction("ViewCourses");
        }

        [HttpPost]
        public ActionResult UpdateCourse(CourseUserModel course)
        {
            if (course.CourseID != 0)
            {
                if (course.StartDate.HasValue)
                {
                    TimeSpan diff = (DateTime)course.EndDate - (DateTime)course.StartDate;
                    if (diff.Days == 0)
                    {
                        TempData["ErrorMessage"] = "Course StartDate cannot be same with EndDate";
                        return View();
                    }
                }
                if (course.EndDate.HasValue)
                {
                    TimeSpan diff = (DateTime)course.EndDate - (DateTime)course.StartDate;
                    if (diff.Days < 0)
                    {
                        TempData["ErrorMessage"] = "Course EndDate cannot be before StartDate";
                        return View();
                    }
                }
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Courses/" + course.CourseID, course).Result;
                TempData["SuccessMessage"] = "Course Updated Successfully";
            }
            return RedirectToAction("ViewCourses");
        }

        // DELETE: Course/Delete
        public ActionResult DeleteCourse(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Courses/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Course Deleted Successfully";
            return RedirectToAction("ViewCourses");
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

        [HttpGet]
        public async Task<ActionResult> SearchCourse(string search)
        {
            List<CourseUserModel> courses = new List<CourseUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Students using HttpClient  
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
                return View(courses.Where(x => x.Name.StartsWith(search) | search == null).ToList());
            }
        }

        // GET: Student/ViewCourses
        public async Task<ActionResult> ViewCoursesEnrollment(int id = 1)  //@TODO: change default to 0 after auth
        {
            List<CourseUserModel> courses = new List<CourseUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);


                client.DefaultRequestHeaders.Clear();
                //TokenManager.CallAPIResource("VGdaYBtkKCqaGTAys_ni_y8jJsBoNa802g7NwaDXQYqK4B3iYha3CMDzMfMNthBYR1jv_YsZHoL6SJJif9ypPlQw8s5MmKpsOZe3YpR_RoHOAG636IP6_0ZW6OAeXB8roal3ADI6t_YohDViKecpIiEetB-hs1BvjXHLHV10Nd1v4PgtCYsYW2aPTY7atpqNAi_TYHRNIyni5fC8Z4qYEAFZx7gce0UbvH0Rvw-Bk2zX9fIDg_wbUBWFApSWPGGTAE2WAx17IzBJWCWhDmyOGY_G44rMtYIetebz57tVqDQ");
                Token tokenFromMgr = TokenManager.GetAccessToken("Rajat", "123");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenFromMgr.AccessToken);

                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
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
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }


                //returning the employee list to view  
                return View(courses);
            }
        }

        // GET: Student/MyCourses
        public async Task<ActionResult> StudentCourses(int id = 1)  //@TODO: change default to 0 after auth
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

                //Sending request to find web api REST service resource Get:Enrollemnts using HttpClient  
                HttpResponseMessage ResFromEnrollment = await client.GetAsync("api/Enrollments/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromEnrollment.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    try
                    {
                        var enrollmentResponse = ResFromEnrollment.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the list  
                        enrollments = JsonConvert.DeserializeObject<List<EnrollmentUserModel>>(enrollmentResponse);

                        var result = enrollments.Where(item => item.StudentID == id).ToList();
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

        // GET: Student/EnrollInCourse
        public async Task<ActionResult> EnrollInCourse(CourseUserModel course)  //@TODO: change default to 0 after auth
        {
            if (course.StartDate.HasValue)
            {
                TimeSpan diff = DateTime.Now - (DateTime)course.StartDate;
                if (diff.Days > 7)
                {
                    TempData["ErrorMessage"] = "Registration for this course is over!";
                    ViewBag.Message = "Registration for this course is over!";
                    return View();
                }
            }
            if (course.EndDate.HasValue)
            {
                TimeSpan diff = DateTime.Now - (DateTime)course.EndDate;
                if (diff.Days > 0)
                {
                    TempData["ErrorMessage"] = "This Course Is Over. We Will Notify You when this course is back";
                    ViewBag.Message = "This Course Is Over. We Will Notify You when this course is back!";
                    return View();
                }
            }
            int StudentID = 1;
            //TODO: Get Student ID from Auth

            EnrollmentUserModel enrollments = new EnrollmentUserModel
            {
                CourseID = course.CourseID,
                StudentID = StudentID
            };

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Enrollemnts using HttpClient  
                HttpResponseMessage ResFromEnrollment = await client.PostAsJsonAsync("api/Enrollments/", enrollments);

                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromEnrollment.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Enrolled Successfully";
                    ViewBag.Message = "Enrolled Successfully!";

                }
                else if (ResFromEnrollment.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ErrorMessage"] = "Already Enrolled!";
                    ViewBag.Message = "Already Enrolled!";
                }

                else
                {

                    TempData["SuccessMessage"] = "There was error enrolling in Course!";
                    ViewBag.Message = "There was error enrolling in Course!";
                }

                return View();
            }
        }

        // GET: Teacher/MyCourses
        public async Task<ActionResult> TeacherCourses(int TeacherId = 1)  //@TODO: change default to 0 after auth
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
                            StartDate = (DateTime)course.StartDate,
                            EndDate = (DateTime)course.EndDate
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

        // GET: Teacher/ViewCourses
        public async Task<ActionResult> ViewCoursesForTeaching()  //@TODO: Id to be changed default to 0 after auth
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

        //GET: Teacher/Save
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
        public async Task<ActionResult> SaveCourses(TeacherCourses courses)
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to Post web api REST service resource using HttpClient  
                HttpResponseMessage ResFromTeaches = await client.PostAsJsonAsync("api/Teaches/", courses);

                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromTeaches.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var teacherCourseResponse = ResFromTeaches.Content.ReadAsStringAsync().Result;
                }
            }
            //returning the Courses list to view  
            return RedirectToAction("TeacherCourses");
        }
    }
}