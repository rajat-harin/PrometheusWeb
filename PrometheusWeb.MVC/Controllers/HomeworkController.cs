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
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Homework/", homework).Result;
                TempData["SuccessMessage"] = "Homework Added Successfully";
            }
            else
            {
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
    }
}