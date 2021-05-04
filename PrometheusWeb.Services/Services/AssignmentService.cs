using PrometheusWeb.Data;
using PrometheusWeb.Data.DataModels;
using PrometheusWeb.Data.UserModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace PrometheusWeb.Services.Services
{
    public class AssignmentService: IAssignmentService
    {
        private PrometheusEntities db;

        public AssignmentService()
        {
            db = new PrometheusEntities();
        }
        public IQueryable<AssignmentUserModel> GetAssignments()
        {
            return db.Assignments.Select(item => new AssignmentUserModel
            {
                AssignmentID = item.AssignmentID,
                HomeWorkID = item.HomeWorkID,
                CourseID = item.CourseID,
                TeacherID = item.TeacherID
            });
        }
        public AssignmentUserModel GetAssignment(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return null;
            }
            AssignmentUserModel userModel = new AssignmentUserModel
            {
                AssignmentID = assignment.AssignmentID,
                HomeWorkID = assignment.HomeWorkID,
                CourseID = assignment.CourseID,
                TeacherID = assignment.TeacherID
            };
            return userModel;
        }
        public bool AddAssignment(AssignmentUserModel userModel)
        {
            Assignment assignment = new Assignment
            {
                AssignmentID = userModel.AssignmentID,
                HomeWorkID = userModel.HomeWorkID,
                CourseID = userModel.CourseID,
                TeacherID = userModel.TeacherID
            };

            
            try
            {
                db.Assignments.Add(assignment);
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }
        public bool UpdateAssignment(int id, AssignmentUserModel userModel)
        {
            Assignment assignment = new Assignment
            {
                AssignmentID = userModel.AssignmentID,
                HomeWorkID = userModel.HomeWorkID,
                CourseID = userModel.CourseID,
                TeacherID = userModel.TeacherID
            };

            if (id != assignment.AssignmentID)
            {
                return false;
            }

            try
            {
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsAssignmentExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
        public AssignmentUserModel DeleteAssignment(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return null;
            }
            try
            {
                db.Assignments.Remove(assignment);
                db.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }
            return new AssignmentUserModel
            {
                AssignmentID = assignment.AssignmentID,
                HomeWorkID = assignment.HomeWorkID,
                CourseID = assignment.CourseID,
                TeacherID = assignment.TeacherID
            };
        }
        public bool IsAssignmentExists(int id)
        {
            return db.Assignments.Count(e => e.AssignmentID == id) > 0;
        }

    }
}