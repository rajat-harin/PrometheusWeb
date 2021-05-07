using Newtonsoft.Json;
using PrometheusWeb.Data.DataModels;
using PrometheusWeb.Data.UserModels;
using PrometheusWeb.Exceptions;
using PrometheusWeb.MVC.Models.ViewModels;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PrometheusWeb.MVC.Controllers
{
    public class HomeworkController : Controller
    {
        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44375/";
        // GET: Homework

        public async Task<ActionResult> Index()
        {
            List<Homework> HomeworkInfo = new List<Homework>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllCourses using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Homework/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var HomeworkResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    HomeworkInfo = JsonConvert.DeserializeObject<List<Homework>>(HomeworkResponse);

                }
                //returning the employee list to view  
                return View(HomeworkInfo);
            }
        }

        // GET: Homework/ViewHomeworks
        public async Task<ActionResult> ViewHomeworks()
        {
            List<HomeworkUserModel> homeworks = new List<HomeworkUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
                HttpResponseMessage ResFromHomeworks = await client.GetAsync("api/Homework/");


                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromHomeworks.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var homeworkResponse = ResFromHomeworks.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the list  
                    homeworks = JsonConvert.DeserializeObject<List<HomeworkUserModel>>(homeworkResponse);

                }
                //returning the employee list to view  
                return View(homeworks);
            }
        }

        public ActionResult AddOrEditHomeworks(int id = 0)
        {
            if (id == 0)

                return View(new HomeworkUserModel());
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Homework/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<HomeworkUserModel>().Result);
            }
        }

        [HttpPost]
        public ActionResult AddOrEditHomeworks(HomeworkUserModel homework)
        {
            if (homework.HomeWorkID == 0)
            {
                if (homework.Deadline.HasValue)
                {
                    TimeSpan diff = (DateTime)homework.Deadline - System.DateTime.Now;
                    if (diff.Days == 0)
                    {
                        TempData["ErrorMessage"] = "Course StartDate cannot be Current Date";
                        return View();
                    }
                }
                if (homework.ReqTime.HasValue)
                {
                    TimeSpan diff = (DateTime)homework.ReqTime - (DateTime)homework.Deadline;
                    if (diff.Days > 0)
                    {
                        TempData["ErrorMessage"] = "Course Required DateTime cannot be beyond Deadline";
                        return View();
                    }
                }
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Homework/", homework).Result;
                TempData["SuccessMessage"] = "Homework Added Successfully";
            }
            else
            {
                if (homework.Deadline.HasValue)
                {
                    TimeSpan diff = (DateTime)homework.Deadline - System.DateTime.Now;
                    if (diff.Days == 0)
                    {
                        TempData["ErrorMessage"] = "Course StartDate cannot be Current Date";
                        return View();
                    }
                }
                if (homework.ReqTime.HasValue)
                {
                    TimeSpan diff = (DateTime)homework.ReqTime - (DateTime)homework.Deadline;
                    if (diff.Days > 0)
                    {
                        TempData["ErrorMessage"] = "Course Required DateTime cannot be beyond Deadline";
                        return View();
                    }
                }
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Homework/" + homework.HomeWorkID, homework).Result;
                TempData["SuccessMessage"] = "Homework Updated Successfully";
            }
            return RedirectToAction("ViewHomeworks");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Homework/" + id.ToString()).Result;
            return RedirectToAction("ViewHomeworks");
        }

        // GET: Student/GetHomeworks
        [Authorize(Roles ="student")]
        public async Task<ActionResult> GetHomeworksOfStudent(int id = 1)
        {
            //string url = "api/h";
            List<AssignedHomework> assignedHomeWord = new List<AssignedHomework>();
            List<EnrollmentUserModel> enrollments = new List<EnrollmentUserModel>();
            List<AssignmentUserModel> assignments = new List<AssignmentUserModel>();
            List<HomeworkUserModel> homeworks = new List<HomeworkUserModel>();

            //Getting Identity Data for Student
            var identity = (ClaimsIdentity)User.Identity;
            var ID = identity.Claims.Where(c => c.Type == "ID")
                        .Select(c => c.Value).FirstOrDefault();
            int studentID;
            try
            {
                if (ID != null)
                {
                    studentID = Int32.Parse(ID);
                }
                else
                {
                    throw new PrometheusWebException("Failed to retrieve ID");
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500);
            }
            var token = identity.Claims.Where(c => c.Type == "AcessToken")
                        .Select(c => c.Value).FirstOrDefault();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Adding Auth Token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
                HttpResponseMessage ResFromEnrollment = await client.GetAsync("api/Enrollments/");
                HttpResponseMessage ResFromAssignment = await client.GetAsync("api/Assignments/");
                HttpResponseMessage ResFromHomework = await client.GetAsync("api/Homework/");
                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromAssignment.IsSuccessStatusCode && ResFromEnrollment.IsSuccessStatusCode && ResFromHomework.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   

                    var enrollmentResponse = ResFromEnrollment.Content.ReadAsStringAsync().Result;
                    var AssignmentResponse = ResFromAssignment.Content.ReadAsStringAsync().Result;
                    var HomeworkResponse = ResFromHomework.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the list  
                    assignments = JsonConvert.DeserializeObject<List<AssignmentUserModel>>(AssignmentResponse);
                    enrollments = JsonConvert.DeserializeObject<List<EnrollmentUserModel>>(enrollmentResponse).Where(item => item.StudentID == id).ToList();
                    homeworks = JsonConvert.DeserializeObject<List<HomeworkUserModel>>(HomeworkResponse);

                    try
                    {
                        var result = from course in enrollments
                                     join assignment in assignments
                                     on course.CourseID equals assignment.CourseID
                                     join homework in homeworks
                                     on assignment.HomeWorkID equals homework.HomeWorkID
                                     select new AssignedHomework
                                     {
                                         AssignmentID = assignment.AssignmentID,
                                         Description = homework.Description,
                                         Deadline = (System.DateTime)homework.Deadline,
                                         ReqTime = (System.DateTime)homework.ReqTime,
                                         LongDescription = homework.LongDescription,
                                         CourseName = course.Course.Name,
                                         HomeworkID = homework.HomeWorkID
                                     };
                        //returning the employee list to view  if list is not empty
                        if (result.Any())
                        {
                            return View(result);
                        }
                        else
                        {
                            return new HttpStatusCodeResult(404);
                        }
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(500);
                    }

                }

                return new HttpStatusCodeResult(404);
            }


        }

        [Authorize(Roles ="student")]
        public async Task<ActionResult> GetHomeworkPlan(int id = 0)  //@TODO: change default to 0 after auth
        {
            //Getting Identity Data for Student
            var identity = (ClaimsIdentity)User.Identity;
            var ID = identity.Claims.Where(c => c.Type == "ID")
                        .Select(c => c.Value).FirstOrDefault();
            int studentID;
            try
            {
                if (ID != null)
                {
                    studentID = Int32.Parse(ID);
                }
                else
                {
                    throw new PrometheusWebException("Failed to retrieve ID");
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500);
            }
            var token = identity.Claims.Where(c => c.Type == "AcessToken")
                        .Select(c => c.Value).FirstOrDefault();
            List<HomeworkPlanUserModel> homeworkPlans = new List<HomeworkPlanUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/HomeworkPlansByStudentID/" + id);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    try
                    {
                        var HomeworkPlanResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the list  
                        homeworkPlans = JsonConvert.DeserializeObject<List<HomeworkPlanUserModel>>(HomeworkPlanResponse);
                        if (homeworkPlans != null)
                        {
                            ViewBag.Message = "";
                            return View(homeworkPlans);
                        }
                        else
                        {

                            ViewBag.Message = "No Plans Found!";
                            return View(homeworkPlans);
                        }
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(500);
                    }

                }

                return new HttpStatusCodeResult(404);
            }

        }

        public async Task<ActionResult> GenerateHomeworkPlan(int id = 1)  //@TODO: change default to 0 after auth
        {


            List<HomeworkPlanUserModel> homeworkPlans = new List<HomeworkPlanUserModel>();
            List<EnrollmentUserModel> enrollments = new List<EnrollmentUserModel>();
            List<AssignmentUserModel> assignments = new List<AssignmentUserModel>();
            List<HomeworkUserModel> homeworks = new List<HomeworkUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
                HttpResponseMessage ResFromEnrollment = await client.GetAsync("api/Enrollments/");
                HttpResponseMessage ResFromAssignment = await client.GetAsync("api/Assignments/");
                HttpResponseMessage ResFromHomework = await client.GetAsync("api/Homework/");
                //Checking the response is successful or not which is sent using HttpClient  
                if (ResFromAssignment.IsSuccessStatusCode && ResFromEnrollment.IsSuccessStatusCode && ResFromHomework.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   

                    var enrollmentResponse = ResFromEnrollment.Content.ReadAsStringAsync().Result;
                    var AssignmentResponse = ResFromAssignment.Content.ReadAsStringAsync().Result;
                    var HomeworkResponse = ResFromHomework.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the list  
                    assignments = JsonConvert.DeserializeObject<List<AssignmentUserModel>>(AssignmentResponse);
                    enrollments = JsonConvert.DeserializeObject<List<EnrollmentUserModel>>(enrollmentResponse).Where(item => item.StudentID == id).ToList();
                    homeworks = JsonConvert.DeserializeObject<List<HomeworkUserModel>>(HomeworkResponse);

                    try
                    {
                        var result = from course in enrollments
                                     join assignment in assignments
                                     on course.CourseID equals assignment.CourseID
                                     join homework in homeworks
                                     on assignment.HomeWorkID equals homework.HomeWorkID
                                     select new AssignedHomework
                                     {
                                         AssignmentID = assignment.AssignmentID,
                                         Description = homework.Description,
                                         Deadline = (System.DateTime)homework.Deadline,
                                         ReqTime = (System.DateTime)homework.ReqTime,
                                         LongDescription = homework.LongDescription,
                                         CourseName = course.Course.Name,
                                         HomeworkID = homework.HomeWorkID
                                     };
                        //returning the employee list to view  if list is not empty
                        if (result != null)
                        {
                            int count = result.Count();
                            homeworkPlans = result.OrderBy(item => item.Deadline).Select(item => new HomeworkPlanUserModel
                            {
                                HomeworkID = item.HomeworkID,
                                isCompleted = false,
                                PriorityLevel = count--,
                                StudentID = id,
                            }).ToList();
                            HttpResponseMessage ResForDeletion = await client.DeleteAsync("api/HomeworkPlans/?StudentID=" + id);
                            if (ResForDeletion.IsSuccessStatusCode)
                            {
                                HttpResponseMessage ResForAdd = await client.PostAsJsonAsync("api/HomeworkPlans/Many", homeworkPlans);
                                if (ResForAdd.IsSuccessStatusCode)
                                {
                                    return RedirectToAction("GetHomeworkPlan", id = 1);
                                }
                                else
                                {
                                    return new HttpStatusCodeResult(500);
                                }
                            }
                        }
                        else
                        {
                            return new HttpStatusCodeResult(404);
                        }
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(500);
                    }

                }

                return new HttpStatusCodeResult(404);
            }

        }

        public async Task<ActionResult> UpdateHomeworkPlan(int id = 1)  //@TODO: change default to 0 after auth
        {
            HomeworkPlanUserModel homeworkPlan = new HomeworkPlanUserModel();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/HomeworkPlans/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    homeworkPlan = JsonConvert.DeserializeObject<HomeworkPlanUserModel>(Response);
                    //returning the employee list to view  
                    return View(homeworkPlan);
                }

                return new HttpStatusCodeResult(404);
            }

        }

        [HttpPost]
        public async Task<ActionResult> UpdateHomeworkPlan(HomeworkPlanUserModel homeworkPlan)  //@TODO: change default to 0 after auth
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PutAsJsonAsync("api/HomeworkPlans/" + homeworkPlan.HomeworkPlanID, homeworkPlan);
                if (Res.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Plan Updated Successfully";
                    return RedirectToAction("GetHomeworkPlan", 1);

                }

                return new HttpStatusCodeResult(404);
            }

        }

    }
}