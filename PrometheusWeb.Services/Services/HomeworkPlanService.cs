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
    public class HomeworkPlanService : IHomeworkPlanService
    {
        private PrometheusEntities db = null;

        public HomeworkPlanService()
        {
            db = new PrometheusEntities();
        }
        public bool AddHomeworkPlan(HomeworkPlanUserModel homeworkPlanModel)
        {
            try
            {
                HomeworkPlan homeworkPlan = new HomeworkPlan
                {
                    HomeworkID = homeworkPlanModel.HomeworkID,
                    StudentID = homeworkPlanModel.StudentID,
                    PriorityLevel = homeworkPlanModel.PriorityLevel,
                    isCompleted = homeworkPlanModel.isCompleted
                };
                db.HomeworkPlans.Add(homeworkPlan);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public HomeworkPlanUserModel DeleteHomeworkPlan(int id)
        {
            HomeworkPlan homeworkPlan = db.HomeworkPlans.Find(id);
            if (homeworkPlan == null)
            {
                return null;
            }

            db.HomeworkPlans.Remove(homeworkPlan);
            db.SaveChanges();

            return new HomeworkPlanUserModel
            {
                HomeworkID = homeworkPlan.HomeworkID,
                StudentID = homeworkPlan.StudentID,
                PriorityLevel = homeworkPlan.PriorityLevel,
                isCompleted = homeworkPlan.isCompleted,
                Homework = new HomeworkUserModel
                {
                    HomeWorkID = homeworkPlan.Homework.HomeWorkID,
                    Description = homeworkPlan.Homework.Description,
                    Deadline = homeworkPlan.Homework.Deadline,
                    ReqTime = homeworkPlan.Homework.ReqTime,
                    LongDescription = homeworkPlan.Homework.LongDescription
                }
            };
        }

        public HomeworkPlanUserModel GetHomeworkPlan(int id)
        {
            try
            {
                HomeworkPlan homeworkPlan = db.HomeworkPlans.Find(id);
                if (homeworkPlan == null)
                {
                    return null;
                }
                return new HomeworkPlanUserModel
                {
                    HomeworkID = homeworkPlan.HomeworkID,
                    StudentID = homeworkPlan.StudentID,
                    PriorityLevel = homeworkPlan.PriorityLevel,
                    isCompleted = homeworkPlan.isCompleted,
                    Homework = new HomeworkUserModel
                    {
                        HomeWorkID = homeworkPlan.Homework.HomeWorkID,
                        Description = homeworkPlan.Homework.Description,
                        Deadline = homeworkPlan.Homework.Deadline,
                        ReqTime = homeworkPlan.Homework.ReqTime,
                        LongDescription = homeworkPlan.Homework.LongDescription
                    }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<HomeworkPlanUserModel> GetHomeworkPlans()
        {
            return db.HomeworkPlans.Select(item => new HomeworkPlanUserModel
            {
                HomeworkID = item.HomeworkID,
                StudentID = item.StudentID,
                PriorityLevel = item.PriorityLevel,
                isCompleted = item.isCompleted,
                Homework = new HomeworkUserModel
                {
                    HomeWorkID = item.Homework.HomeWorkID,
                    Description = item.Homework.Description,
                    Deadline = item.Homework.Deadline,
                    ReqTime = item.Homework.ReqTime,
                    LongDescription = item.Homework.LongDescription
                }
            });
        }

        public IQueryable<HomeworkPlanUserModel> GetHomeworkPlans(int StudentID)
        {
            throw new NotImplementedException();
        }

        public bool IsHomeworkPlanExists(int id)
        {
            return db.HomeworkPlans.Count(e => e.HomeworkPlanID == id) > 0;
        }

        public bool UpdateHomeworkPlan(int id, HomeworkPlanUserModel homeworkPlanModel)
        {
            HomeworkPlan homeworkPlan = new HomeworkPlan
            {
                HomeworkID = homeworkPlanModel.HomeworkID,
                StudentID = homeworkPlanModel.StudentID,
                PriorityLevel = homeworkPlanModel.PriorityLevel,
                isCompleted = homeworkPlanModel.isCompleted
            };

            if (id != homeworkPlan.HomeworkPlanID)
            {
                return false;
            }

            db.Entry(homeworkPlan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsHomeworkPlanExists(id))
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
    }
}