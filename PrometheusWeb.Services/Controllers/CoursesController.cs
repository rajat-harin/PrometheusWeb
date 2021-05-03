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

namespace PrometheusWeb.Services.Controllers
{
    public class CoursesController : ApiController
    {
        private PrometheusEntities db = new PrometheusEntities();

        // GET: api/Courses
        public IQueryable<CourseUserModel> GetCourses()
        {

            return db.Courses.Select(item => new CourseUserModel
            {
                CourseID = item.CourseID,
                Name = item.Name,
                StartDate = item.StartDate,
                EndDate = item.EndDate
            }) ;
        }

        // GET: api/Courses/5
        [ResponseType(typeof(CourseUserModel))]
        public IHttpActionResult GetCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            CourseUserModel courseUser = new CourseUserModel
            {
                CourseID = course.CourseID,
                Name = course.Name,
                StartDate = course.StartDate,
                EndDate = course.EndDate
            };
            return Ok(courseUser);
        }

        // PUT: api/Courses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCourse(int id, CourseUserModel courseModel)
        {
            Course course = new Course
            {
                CourseID = courseModel.CourseID,
                Name = courseModel.Name,
                StartDate = courseModel.StartDate,
                EndDate = courseModel.EndDate
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.CourseID)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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
        [ResponseType(typeof(Course))]
        public IHttpActionResult PostCourse(CourseUserModel courseModel)
        {
            Course course = new Course
            {
                CourseID = courseModel.CourseID,
                Name = courseModel.Name,
                StartDate = courseModel.StartDate,
                EndDate = courseModel.EndDate
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(course);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = course.CourseID }, course);
        }

        // DELETE: api/Courses/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult DeleteCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            db.Courses.Remove(course);
            db.SaveChanges();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.CourseID == id) > 0;
        }
    }
}