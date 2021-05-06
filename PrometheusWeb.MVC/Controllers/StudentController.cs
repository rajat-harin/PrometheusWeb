using Newtonsoft.Json;
using PrometheusWeb.Data.UserModels;
using PrometheusWeb.MVC.Models.ViewModels;
using PrometheusWeb.Utilities;
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
    public class StudentController : Controller
    {
        //Hosted web API REST Service base url  
        const string BaseURL = "https://localhost:44375/";
        // GET: Student
        
        public async Task<ActionResult> Index()
        {

            return View();
        }
        // GET: Student/MyCourses
        public async Task<ActionResult> MyCourses(int id = 1)  //@TODO: change default to 0 after auth
        {
            List<CourseUserModel> courses = new List<CourseUserModel>();
            List<EnrollmentUserModel> enrollments = new List<EnrollmentUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseURL);

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

        // GET: Student/ViewCourses
        public async Task<ActionResult> ViewCourses(int id = 1)  //@TODO: change default to 0 after auth
        {
            List<CourseUserModel> courses = new List<CourseUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseURL);

                
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
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                
                //returning the employee list to view  
                return View(courses);
            }
        }

        // GET: Student/GetHomeworks
        public async Task<ActionResult> GetHomeworks(int id = 1)
        {
            //string url = "api/h";
            List<AssignedHomework> assignedHomeWord = new List<AssignedHomework>();
            List<EnrollmentUserModel> enrollments = new List<EnrollmentUserModel>();
            List<AssignmentUserModel> assignments = new List<AssignmentUserModel>();
            List<HomeworkUserModel> homeworks = new List<HomeworkUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseURL);

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
        // GET: Student/EnrollInCourse
        public async Task<ActionResult> EnrollInCourse(CourseUserModel course)  //@TODO: change default to 0 after auth
        {
            if (course.StartDate.HasValue)
            {
                TimeSpan diff = DateTime.Now - (DateTime)course.StartDate;
                if (diff.Days > 7)
                {
                    TempData["ErrorMessage"] = "Course Already Exists";
                    ViewBag.Message = "Registration for this course is over!";
                    return View();
                }
            }
            int StudentID = 1;
            //TODO: Get Student ID from Auth

            EnrollmentUserModel enrollments = new EnrollmentUserModel {
                CourseID = course.CourseID,
                StudentID = StudentID
            };

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseURL);

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

        public async Task<ActionResult> GetHomeworkPlan(int id = 1)  //@TODO: change default to 0 after auth
        {
            
            List<HomeworkPlanUserModel> homeworkPlans = new List<HomeworkPlanUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseURL);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Get:Courses & Get:Enrollemnts using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/HomeworkPlansByStudentID/"+ id);
               
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

        public async Task<ActionResult> GeneratePlan(int id = 1)  //@TODO: change default to 0 after auth
        {

            
            List<HomeworkPlanUserModel> homeworkPlans = new List<HomeworkPlanUserModel>();
            List<EnrollmentUserModel> enrollments = new List<EnrollmentUserModel>();
            List<AssignmentUserModel> assignments = new List<AssignmentUserModel>();
            List<HomeworkUserModel> homeworks = new List<HomeworkUserModel>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseURL);

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
                            homeworkPlans =  result.OrderBy(item => item.Deadline).Select(item => new HomeworkPlanUserModel
                            {
                                HomeworkID = item.HomeworkID,
                                isCompleted = false,
                                PriorityLevel = count--,
                                StudentID = id,
                            }).ToList();
                            HttpResponseMessage ResForDeletion = await client.DeleteAsync("api/HomeworkPlans/?StudentID=" + id);
                            if(ResForDeletion.IsSuccessStatusCode)
                            {
                                HttpResponseMessage ResForAdd = await client.PostAsJsonAsync("api/HomeworkPlans/Many", homeworkPlans);
                                if(ResForAdd.IsSuccessStatusCode)
                                {
                                    return RedirectToAction("GetHomeworkPlan", id = 1 );
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

        public async Task<ActionResult> UpdatePlan(int id = 1)  //@TODO: change default to 0 after auth
        {
            HomeworkPlanUserModel homeworkPlan = new HomeworkPlanUserModel();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseURL);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/HomeworkPlans/"+id);
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
        public async Task<ActionResult> UpdatePlan(HomeworkPlanUserModel homeworkPlan)  //@TODO: change default to 0 after auth
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseURL);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PutAsJsonAsync("api/HomeworkPlans/"+homeworkPlan.HomeworkPlanID,homeworkPlan);
                if (Res.IsSuccessStatusCode)
                {
                    
                   return RedirectToAction("GetHomeworkPlan", 1);
                   
                }

                return new HttpStatusCodeResult(404);
            }

        }
    }
}
