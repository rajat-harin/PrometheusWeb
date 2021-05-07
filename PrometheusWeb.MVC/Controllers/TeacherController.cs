﻿using Newtonsoft.Json;
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
                    if (user.DOB > DateTime.Now)
                    {
                        TempData["ErrorMessage"] = "DOB cannot be CurrentDate or after CurrentDate";
                        ViewBag.Message = "DOB cannot be same with CurrentDate";
                        return View();
                    }
                }
                HttpResponseMessage responseUser = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Users/", user).Result;

                HttpResponseMessage responseStudent = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Teachers/", user).Result;

                if (responseUser.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Student Added Successfully";
                    ViewBag.Message = "Student Added Successfully";

                }
                else if (responseUser.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ErrorMessage"] = "UserID Already Taken";
                    ViewBag.Message = "UserID Already Taken";
                }

                else
                {
                    TempData["ErrorMessage"] = "There was error registering a Student!";
                    ViewBag.Message = "There was error registering a Student!";
                }

                if (responseStudent.IsSuccessStatusCode)
                {
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
            return RedirectToAction("AddTeacher");
        }

        // DELETE: Admin/DeleteTeacher
        public ActionResult DeleteTeacher(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Teachers/" + id.ToString()).Result;
            HttpResponseMessage responseUser = GlobalVariables.WebApiClient.DeleteAsync("api/Users/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Teacher Deleted Successfully";
            return RedirectToAction("ViewTeachers");
        }

        // POST: Admin/EditTeacherProfile
        public ActionResult UpdateTeacher(int id = 0)
        {
            if (id != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Teachers/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<TeacherUserModel>().Result);
            }
            return RedirectToAction("ViewTeachers");
        }

        [HttpPost]
        public ActionResult UpdateTeacher(TeacherUserModel teacher)
        {
            if (teacher.TeacherID != 0)
            {
                if (teacher.DOB.HasValue)
                {
                    TimeSpan diff = DateTime.Now - (DateTime)teacher.DOB;
                    if (diff.Days == 0)
                    {
                        TempData["ErrorMessage"] = "DOB cannot be same with CurrentDate";
                        ViewBag.Message = "DOB cannot be same with CurrentDate";
                        return View();
                    }
                    if (teacher.DOB > DateTime.Now)
                    {
                        TempData["ErrorMessage"] = "DOB cannot be CurrentDate or after CurrentDate";
                        ViewBag.Message = "DOB cannot be same with CurrentDate";
                        return View();
                    }
                }
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Teachers/" + teacher.TeacherID, teacher).Result;
                TempData["SuccessMessage"] = "Teacher Updated Successfully";
            }
            return RedirectToAction("ViewTeachers");
        }

        public ActionResult UpdateTeacherProfile(int id = 1)
        {
            if (id != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Teachers/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<TeacherUserModel>().Result);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateTeacherProfile(TeacherUserModel teacher)
        {
            if (teacher.TeacherID != 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Teachers/" + teacher.TeacherID, teacher).Result;
                TempData["SuccessMessage"] = "Teacher Updated Successfully";
            }
            return RedirectToAction("Index");
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