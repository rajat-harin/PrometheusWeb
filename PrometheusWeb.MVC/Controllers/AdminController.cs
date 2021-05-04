using Newtonsoft.Json;
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


        // GET: Admin/ViewStudents
        public async Task<ActionResult> ViewStudents(string id = "")
        {
            List<StudentUserModel> students = new List<StudentUserModel>();
            List<AdminUserModel> users = new List<AdminUserModel>();
            
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
                HttpResponseMessage ResFromStudents = await client.GetAsync("api/Students/");
                HttpResponseMessage ResFromUsers = await client.GetAsync("api/Users/");


                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromStudents.IsSuccessStatusCode && ResFromUsers.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var studentResponse = ResFromStudents.Content.ReadAsStringAsync().Result;
                    var userResponse = ResFromUsers.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the list  
                    students = JsonConvert.DeserializeObject<List<StudentUserModel>>(studentResponse);
                    users = JsonConvert.DeserializeObject<List<AdminUserModel>>(userResponse);
                    
                    try
                    {
                        var result = users.Join(
                    students,
                    user => user.UserID,
                    student => student.UserID,
                    (user, student) => new AdminUserModel
                    { 
                        StudentID = student.StudentID,
                        FName = student.FName,
                        LName = student.LName,
                        UserID = student.UserID,
                        DOB = student.DOB,
                        Address = student.Address,
                        City = student.City,
                        MobileNo = student.MobileNo,
                        Password = user.Password,
                        Role = user.Role,
                        SecurityQuestion = user.SecurityQuestion,
                        SecurityAnswer = user.SecurityAnswer
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

            public ActionResult AddOrEditStudent(string id = "")
            {
            if (id == "")
                return View(new AdminUserModel());

            else
            {
                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.GetAsync("api/Students/" + id.ToString()).Result;
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.GetAsync("api/Users/" + id.ToString()).Result;
                return View(responseUser.Content.ReadAsAsync<AdminUserModel>().Result);
            }
        }
        [HttpPost]
        public ActionResult AddOrEditStudent(AdminUserModel user)
        {
            if (user.UserID == "")
            {
                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Students/", user).Result;
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Users/", user).Result;
                TempData["SuccessMessage"] = "Student Added Successfully";
            }
            else
            {
                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Students/" + user.UserID, user).Result;
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Users/" + user.UserID, user).Result;
                TempData["SuccessMessage"] = "Student Updated Successfully";
            }
            return RedirectToAction("ViewStudents");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Users" + id.ToString()).Result;
            return RedirectToAction("ViewStudents");
        }
    }
}