using PrometheusWeb.Data;
using PrometheusWeb.Data.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrometheusWeb.Services.Services
{
    public class TeachesService : ITeachesService
    {
        private PrometheusEntities db = null;
        public TeachesService()
        {
            db = new PrometheusEntities();
        }
        public bool AddTeacherCourse(TeacherCourseUserModel teacherCourseModel)
        {
            throw new NotImplementedException();
        }

        public TeacherCourseUserModel DeleteTeacherCourse(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TeacherCourseUserModel> GetTeacherCourses()
        {
            return db.Teaches.Select(item => new TeacherCourseUserModel
            {
                CourseID = item.CourseID,
                TeacherCourseID = item.TeacherCourseID,
                TeacherID = item.TeacherID
            });
        }

        public TeacherCourseUserModel GetTeacherCourses(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsTeachesExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTeacherCourses(int id, TeacherCourseUserModel teacherCourseModel)
        {
            throw new NotImplementedException();
        }
    }
}