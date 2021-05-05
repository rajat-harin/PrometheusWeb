using PrometheusWeb.Data.DataModels;
using System.Linq;
using System.Web.Http;
using PrometheusWeb.Data.UserModels;

namespace PrometheusWeb.Services.Services
{
    public interface IHomeworkPlanService
    {
        HomeworkPlanUserModel DeleteHomeworkPlan(int id);
        HomeworkPlanUserModel GetHomeworkPlan(int id);
        IQueryable<HomeworkPlanUserModel> GetHomeworkPlans(int StudentID);
        IQueryable<HomeworkPlanUserModel> GetHomeworkPlans();
        bool AddHomeworkPlan(HomeworkPlanUserModel homeworkPlanModel);
        bool UpdateHomeworkPlan(int id, HomeworkPlanUserModel homeworkPlanModel);
        bool IsHomeworkPlanExists(int id);
    }
}