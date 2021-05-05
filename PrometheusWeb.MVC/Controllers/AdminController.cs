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
        public ActionResult Index()
        {
            return View();
        }


        // GET: Course/ViewStudents
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

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
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

        public ActionResult AddStudent(int id = 0)
        {
            if (id == 0)
            {
                return View(new AdminUserModel());
            }
            else
            {
                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.GetAsync("api/Students/" + id.ToString()).Result;
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.GetAsync("api/Users/" + id.ToString()).Result;
                return View(responseUser.Content.ReadAsAsync<AdminUserModel>().Result);
            }
        }
        [HttpPost]
        public ActionResult AddStudent(AdminUserModel user)
        {
            if (user.StudentID == 0)
            {
                user.Role = "student";
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Users/", user).Result;
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

        public ActionResult DeleteStudent(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Students/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Student Deleted Successfully";
            return RedirectToAction("ViewStudents");
        }

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

        public ActionResult AddTeacher(int id = 0)
        {
            if (id == 0)
            {
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

        public ActionResult DeleteTeacher(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Students/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Teacher Deleted Successfully";
            return RedirectToAction("ViewTeachers");
        }
    }
}