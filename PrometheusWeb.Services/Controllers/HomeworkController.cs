using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PrometheusWeb.Data;
using PrometheusWeb.Data.DataModels;
using PrometheusWeb.Data.UserModels;
using PrometheusWeb.Services.Services;

namespace PrometheusWeb.Services.Controllers
{
    public class HomeworkController : ApiController
    {
        private IHomeworkService _homeworkService = null;
        public HomeworkController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }

        // GET: api/Homework
        public IQueryable<HomeworkUserModel> GetHomeworks()
        {
            return _homeworkService.GetHomeworks();
        }

        // GET: api/Homework/5
        [ResponseType(typeof(HomeworkUserModel))]
        public IHttpActionResult GetHomework(int id)
        {
            HomeworkUserModel homework = _homeworkService.GetHomework(id);
            if (homework == null)
            {
                return NotFound();
            }
            return Ok(homework);
        }

        // PUT: api/Homework/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHomework(int id, HomeworkUserModel homeworkModel)
        {
            bool result;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                result = _homeworkService.UpdateHomework(id, homeworkModel);
            }
            catch (Exception)
            {
                if (!_homeworkService.ifHomeworkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Homework
        [ResponseType(typeof(Homework))]
        public IHttpActionResult PostHomework(HomeworkUserModel homeworkModel)
        {
            var result = _homeworkService.AddHomework(homeworkModel);
            if (result)
                return CreatedAtRoute("DefaultApi", new { id = homeworkModel.HomeWorkID }, homeworkModel);
            else
                return StatusCode(HttpStatusCode.InternalServerError);
        }

        // DELETE: api/Homework/5
        [ResponseType(typeof(Homework))]
        public IHttpActionResult DeleteHomework(int id)
        {
            HomeworkUserModel homework = _homeworkService.DeleteHomework(id);

            return Ok(homework);
        }
    }
}