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
    public class StudentsController : ApiController
    {
        private PrometheusEntities db = new PrometheusEntities(); // Remove after DI
        private IStudentService _studentService;
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Students
        public IQueryable<StudentUserModel> GetStudents()
        {
            return _studentService.GetStudents();
        }

        // GET: api/Students/5
        [ResponseType(typeof(Student))]
        public IHttpActionResult GetStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            StudentUserModel studentUser = new StudentUserModel
            {
                StudentID = student.StudentID,
                FName = student.FName,
                LName = student.LName,
                UserID = student.UserID,
                DOB = student.DOB,
                Address = student.Address,
                City = student.City,
                MobileNo = student.MobileNo
            };
            return Ok(student);
        }

        // PUT: api/Students/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudent(int id, StudentUserModel studentModel)
        {
            Student student = new Student
            {
                StudentID = studentModel.StudentID,
                FName = studentModel.FName,
                LName = studentModel.LName,
                UserID = studentModel.UserID,
                DOB = studentModel.DOB,
                Address = studentModel.Address,
                City = studentModel.City,
                MobileNo = studentModel.MobileNo
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.StudentID)
            {
                return BadRequest();
            }

            db.Entry(student).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        [ResponseType(typeof(Student))]
        public IHttpActionResult PostStudent(StudentUserModel studentModel)
        {
            Student student = new Student
            {
                StudentID = studentModel.StudentID,
                FName = studentModel.FName,
                LName = studentModel.LName,
                UserID = studentModel.UserID,
                DOB = studentModel.DOB,
                Address = studentModel.Address,
                City = studentModel.City,
                MobileNo = studentModel.MobileNo
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(student);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = student.StudentID }, student);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(Student))]
        public IHttpActionResult DeleteStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            db.Students.Remove(student);
            db.SaveChanges();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.StudentID == id) > 0;
        }
    }
}