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
    public class TeachersController : ApiController
    {
        private PrometheusEntities db = new PrometheusEntities();

        // GET: api/Teachers
        public IQueryable<TeacherUserModel> GetTeachers()
        {
            return db.Teachers.Select(item => new TeacherUserModel
            {
                TeacherID = item.TeacherID,
                FName = item.FName,
                LName = item.LName,
                UserID = item.UserID,
                DOB = item.DOB,
                Address = item.Address,
                City = item.City,
                MobileNo = item.MobileNo,
                IsAdmin = item.IsAdmin
            });
        }

        // GET: api/Teachers/5
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult GetTeacher(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }
            TeacherUserModel teachertUser = new TeacherUserModel
            {
                TeacherID = teacher.TeacherID,
                FName = teacher.FName,
                LName = teacher.LName,
                UserID = teacher.UserID,
                DOB = teacher.DOB,
                Address = teacher.Address,
                City = teacher.City,
                MobileNo = teacher.MobileNo,
                IsAdmin = teacher.IsAdmin
            };
            return Ok(teacher);
        }

        // PUT: api/Teachers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeacher(int id, TeacherUserModel teacherModel)
        {
            Teacher teacher = new Teacher
            {
                TeacherID = teacherModel.TeacherID,
                FName = teacherModel.FName,
                LName = teacherModel.LName,
                UserID = teacherModel.UserID,
                DOB = teacherModel.DOB,
                Address = teacherModel.Address,
                City = teacherModel.City,
                MobileNo = teacherModel.MobileNo,
                IsAdmin = teacherModel.IsAdmin
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacher.TeacherID)
            {
                return BadRequest();
            }

            db.Entry(teacher).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        // POST: api/Teachers
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult PostTeacher(TeacherUserModel teacherModel)
        {
            Teacher teacher = new Teacher
            {
                TeacherID = teacherModel.TeacherID,
                FName = teacherModel.FName,
                LName = teacherModel.LName,
                UserID = teacherModel.UserID,
                DOB = teacherModel.DOB,
                Address = teacherModel.Address,
                City = teacherModel.City,
                MobileNo = teacherModel.MobileNo,
                IsAdmin = teacherModel.IsAdmin
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teachers.Add(teacher);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = teacher.TeacherID }, teacher);
        }

        // DELETE: api/Teachers/5
        [ResponseType(typeof(Teacher))]
        public IHttpActionResult DeleteTeacher(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }

            db.Teachers.Remove(teacher);
            db.SaveChanges();

            return Ok(teacher);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherExists(int id)
        {
            return db.Teachers.Count(e => e.TeacherID == id) > 0;
        }
    }
}