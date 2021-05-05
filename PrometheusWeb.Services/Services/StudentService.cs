using PrometheusWeb.Data;
using PrometheusWeb.Data.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrometheusWeb.Services.Services
{
    public class StudentService : IStudentService
    {
        private PrometheusEntities db;

        //Constructor instatiating db Context
        public StudentService()
        {
            db = new PrometheusEntities();
        }
        public bool AddStudent(StudentUserModel studentModel)
        {
            throw new NotImplementedException();
        }

        public StudentUserModel DeleteStudent(int id)
        {
            throw new NotImplementedException();
        }

        public StudentUserModel GetStudent(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentUserModel> GetStudents()
        {
            return db.Students.Select(item => new StudentUserModel
            {
                StudentID = item.StudentID,
                FName = item.FName,
                LName = item.LName,
                UserID = item.UserID,
                DOB = item.DOB,
                Address = item.Address,
                City = item.City,
                MobileNo = item.MobileNo
            });
        }

        public bool IsStudentExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateStudent(int id, StudentUserModel studentModel)
        {
            throw new NotImplementedException();
        }
    }
}