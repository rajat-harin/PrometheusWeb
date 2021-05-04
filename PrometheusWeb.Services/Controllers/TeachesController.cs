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
    public class TeachesController : ApiController
    {
        private PrometheusEntities db = new PrometheusEntities();
        private ITeachesService _teachesService = null;

        public TeachesController(ITeachesService teachesService)
        {
            _teachesService = teachesService;
        }

        // GET: api/Teaches
        public IQueryable<TeacherCourseUserModel> GetTeacherCourses()
        {
            return _teachesService.GetTeacherCourses();
        }

        // GET: api/Teaches/5
        [ResponseType(typeof(Teach))]
        public IHttpActionResult GetTeacherCourses(int id)
        {
            Teach teachObj = db.Teaches.Find(id);
            if (teachObj == null)
            {
                return NotFound();
            }
            return Ok(teachObj);
        }

        // PUT: api/Teaches/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeacherCourses(int id, Teach teach)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teach.TeacherCourseID)
            {
                return BadRequest();
            }

            db.Entry(teach).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherCourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Courses
        [ResponseType(typeof(Teach))]
        public IHttpActionResult PostTeacherCourse(TeacherCourseUserModel courseModel)
        {
            Teach teach = new Teach
            {
                CourseID = courseModel.CourseID,
                TeacherID = courseModel.TeacherID
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teaches.Add(teach);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = teach.CourseID }, teach);
        }

        // DELETE: api/Teaches/5
        [ResponseType(typeof(Teach))]
        public IHttpActionResult DeleteTeacherCourse(int id)
        {
            Teach teach = db.Teaches.Find(id);
            if (teach == null)
            {
                return NotFound();
            }

            db.Teaches.Remove(teach);
            db.SaveChanges();

            return Ok(teach);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherCourseExists(int id)
        {
            return db.Teaches.Count(e => e.TeacherCourseID == id) > 0;
        }
    }
}