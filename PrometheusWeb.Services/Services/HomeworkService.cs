using PrometheusWeb.Data.DataModels;
using PrometheusWeb.Data.UserModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using PrometheusWeb.Data;
using System.Data.Entity;

namespace PrometheusWeb.Services.Services
{
    public class HomeworkService : IHomeworkService
    {
        public bool AddHomework(HomeworkUserModel homeworkModel)
        {
            throw new NotImplementedException();
        }

        public HomeworkUserModel DeleteHomework(int id)
        {
            throw new NotImplementedException();
        }

        public HomeworkUserModel GetHomework(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<HomeworkUserModel> GetHomeworks()
        {
            throw new NotImplementedException();
        }

        public bool ifHomeworkExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateHomework(int id, HomeworkUserModel homeworkModel)
        {
            throw new NotImplementedException();
        }
    }
}