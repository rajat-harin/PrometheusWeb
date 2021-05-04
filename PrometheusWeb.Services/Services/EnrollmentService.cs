﻿using PrometheusWeb.Data;
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
    public class EnrollmentService : IEnrollmentService
    {
        private PrometheusEntities db;

        public EnrollmentService()
        {
            db = new PrometheusEntities();
        }
        public IQueryable<EnrollmentUserModel> GetEnrollments()
        {
            return db.Enrollments.Select(item => new EnrollmentUserModel
            {
                EnrollmentID = item.EnrollmentID,
                CourseID = item.CourseID,
                StudentID = item.StudentID
            });
        }
        public EnrollmentUserModel GetEnrollment(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return null;
            }
            EnrollmentUserModel enrolledUser = new EnrollmentUserModel
            {
                EnrollmentID = enrollment.EnrollmentID,
                CourseID = enrollment.CourseID,
                StudentID = enrollment.StudentID
            };
            return enrolledUser;
        }
        public bool AddEnrollment(EnrollmentUserModel enrollmentModel)
        {
            Enrollment enrollment = new Enrollment
            {

                EnrollmentID = enrollmentModel.EnrollmentID,
                CourseID = enrollmentModel.CourseID,
                StudentID = enrollmentModel.StudentID
            };

            db.Enrollments.Add(enrollment);
            db.SaveChanges();

            return true;
        }
        public bool UpdateEnrollment(int id, EnrollmentUserModel enrollmentModel)
        {
            Enrollment enrollment = new Enrollment
            {
                
                EnrollmentID = enrollmentModel.EnrollmentID,
                CourseID = enrollmentModel.CourseID,
                StudentID = enrollmentModel.StudentID
            };

            if (id != enrollment.EnrollmentID)
            {
                return false;
            }

            db.Entry(enrollment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(id))
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
        public EnrollmentUserModel DeleteEnrollment(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return null;
            }

            db.Enrollments.Remove(enrollment);
            db.SaveChanges();

            return new EnrollmentUserModel
            {
                EnrollmentID = enrollment.EnrollmentID,
                CourseID = enrollment.CourseID,
                StudentID = enrollment.StudentID
            };
        }
        public bool EnrollmentExists(int id)
        {
            return db.Enrollments.Count(e => e.EnrollmentID == id) > 0;
        }
        
    }
}