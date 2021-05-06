using Newtonsoft.Json;
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
    public class AdminController : Controller
    {
        //Hosted web API REST Service base url
        string Baseurl = "https://localhost:44375/";

        // GET: Admin
        public ActionResult Index(string search)
        {
            return View();
        }

        // GET: Admin/ViewStudents
        public async Task<ActionResult> ViewStudents()
        {
            List<StudentUserModel> students = new List<StudentUserModel>();
            
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Students using HttpClient  
                HttpResponseMessage ResFromCourses = await client.GetAsync("api/Students/");


                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromCourses.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var studentResponse = ResFromCourses.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the list  
                    students = JsonConvert.DeserializeObject<List<StudentUserModel>>(studentResponse);

                }
                //returning the employee list to view  
                return View(students);
            }
        }

        // POST: Admin/AddStudent
        public ActionResult AddStudent(int id = 0)
        {
            if (id == 0)
            {
                var list = new List<string>() { "What is your Pet Name?", "What is your Nick Name", "What is your School Name?" };
                ViewBag.list = list;
                return View(new AdminUserModel());
            }
            else
            {
                //Sending request to find web api REST service resource Get:Students using HttpClient
                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.GetAsync("api/Students/" + id.ToString()).Result;
                //Sending request to find web api REST service resource Get:Users using HttpClient
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.GetAsync("api/Users/" + id.ToString()).Result;
                //Storing the response details recieved from web api   
                return View(responseUser.Content.ReadAsAsync<AdminUserModel>().Result);
            }
        }

        [HttpPost]
        public ActionResult AddStudent(AdminUserModel user)
        {
            if (user.StudentID == 0)
            {
                user.Role = "student";
                var list = new List<string>() { "What is your Pet Name?", "What is your Nick Name", "What is your School Name?" };
                ViewBag.list = list;
                if (user.DOB.HasValue)
                {
                    TimeSpan diff = DateTime.Now - (DateTime)user.DOB;
                    if (diff.Days == 0)
                    {
                        TempData["ErrorMessage"] = "DOB cannot be same with CurrentDate";
                        ViewBag.Message = "DOB cannot be same with CurrentDate";
                        return View();
                    }
                }
                //Sending request to find web api REST service resource Post:Users using HttpClient
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Users/", user).Result;
                //Sending request to find web api REST service resource Post: using HttpClient
                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Students/", user).Result;
                
                TempData["SuccessMessage"] = "Student Added Successfully";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Students/" + user.StudentID, user).Result;
                TempData["SuccessMessage"] = "Student Updated Successfully";
            }
            return RedirectToAction("ViewStudents");
        }

        // DELETE: Admin/DeleteStudent
        public ActionResult DeleteStudent(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Students/" + id.ToString()).Result;
            HttpResponseMessage responseUser = GlobalVariables.WebApiClient.DeleteAsync("api/Users/" + id.ToString()).Result;
            return RedirectToAction("ViewStudents");
        }

        // GET: Admin/ViewTeachers
        public async Task<ActionResult> ViewTeachers()
        {
            List<TeacherUserModel> teachers = new List<TeacherUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
                HttpResponseMessage ResFromCourses = await client.GetAsync("api/Teachers/");


                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromCourses.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var teacherResponse = ResFromCourses.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the list  
                    teachers = JsonConvert.DeserializeObject<List<TeacherUserModel>>(teacherResponse);

                }
                //returning the employee list to view  
                return View(teachers);
            }
        }

        // POST: Admin/AddTeacher
        public ActionResult AddTeacher(int id = 0)
        {
            if (id == 0)
            {
                var list = new List<string>() { "What is your Pet Name?", "What is your Nick Name", "What is your School Name?" };
                ViewBag.list = list;
                return View(new AdminUserModel());
            }
            else
            {
                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.GetAsync("api/Teachers/" + id.ToString()).Result;
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.GetAsync("api/Users/" + id.ToString()).Result;
                return View(responseUser.Content.ReadAsAsync<AdminUserModel>().Result);
            }
        }

        [HttpPost]
        public ActionResult AddTeacher(AdminUserModel user)
        {
            if (user.TeacherID == 0)
            {
                if (user.IsAdmin == true)
                {
                    user.Role = "admin";
                }
                else
                {
                    user.Role = "teacher";
                }
                var list = new List<string>() { "What is your Pet Name?", "What is your Nick Name", "What is your School Name?" };
                ViewBag.list = list;
                if (user.DOB.HasValue)
                {
                    TimeSpan diff = DateTime.Now - (DateTime)user.DOB;
                    if (diff.Days == 0)
                    {
                        TempData["ErrorMessage"] = "DOB cannot be same with CurrentDate";
                        ViewBag.Message = "DOB cannot be same with CurrentDate";
                        return View();
                    }
                }
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Users/", user).Result;
                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Teachers/", user).Result;
                
                TempData["SuccessMessage"] = "Teacher Added Successfully";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Teachers/" + user.TeacherID, user).Result;
                TempData["SuccessMessage"] = "Teacher Updated Successfully";
            }
            return RedirectToAction("ViewTeachers");
        }

        // DELETE: Admin/DeleteTeacher
        public ActionResult DeleteTeacher(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Teachers/" + id.ToString()).Result;
            HttpResponseMessage responseUser = GlobalVariables.WebApiClient.DeleteAsync("api/Users/" + id.ToString()).Result;
            return RedirectToAction("ViewTeachers");
        }

        // POST: Admin/EditTeacherProfile
        public ActionResult EditTeacher(int id = 0)
        {
            if (id != 0)
            { 
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Teachers/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<TeacherUserModel>().Result);
            }
            return RedirectToAction("ViewTeachers");
        }

        [HttpPost]
        public ActionResult EditTeacher(TeacherUserModel teacher)
        {
            if (teacher.TeacherID != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Teachers/" + teacher.TeacherID, teacher).Result;
                TempData["SuccessMessage"] = "Teacher Updated Successfully";
            }
            return RedirectToAction("ViewTeachers");
        }

        // POST: Admin/EditStudentProfile
        public ActionResult EditStudent(int id = 0)
        {
            if (id != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Students/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<StudentUserModel>().Result);
            }
            return RedirectToAction("ViewStudents");
        }

        [HttpPost]
        public ActionResult EditStudent(StudentUserModel student)
        {
            if (student.StudentID != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Students/" + student.StudentID, student).Result;
                TempData["SuccessMessage"] = "Student Updated Successfully";
            }
            return RedirectToAction("ViewStudents");
        }

        [HttpGet]
        public async Task<ActionResult> SearchStudent(string search)
        {
            List<StudentUserModel> students = new List<StudentUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Students using HttpClient  
                HttpResponseMessage ResFromCourses = await client.GetAsync("api/Students/");


                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromCourses.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var studentResponse = ResFromCourses.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the list  
                    students = JsonConvert.DeserializeObject<List<StudentUserModel>>(studentResponse);

                }
                //returning the employee list to view  
                return View(students.Where(x => x.FName.StartsWith(search) | search == null).ToList());
            }
        }

        [HttpGet]
        public async Task<ActionResult> SearchTeacher(string search)
        {
            List<TeacherUserModel> teachers = new List<TeacherUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Students using HttpClient  
                HttpResponseMessage ResFromCourses = await client.GetAsync("api/Teachers/");


                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromCourses.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var teacherResponse = ResFromCourses.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the list  
                    teachers = JsonConvert.DeserializeObject<List<TeacherUserModel>>(teacherResponse);

                }
                //returning the employee list to view  
                return View(teachers.Where(x => x.FName.StartsWith(search) | search == null).ToList());
            }
        }
    }
}