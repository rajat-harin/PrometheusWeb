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

namespace PrometheusWeb.Services.Controllers
{
    public class HomeworkPlansController : ApiController
    {
        private PrometheusEntities db = new PrometheusEntities();

        // GET: api/HomeworkPlans
        public IQueryable<HomeworkPlan> GetHomeworkPlans()
        {
            return db.HomeworkPlans;
        }

        // GET: api/HomeworkPlans/5
        [ResponseType(typeof(HomeworkPlan))]
        public IHttpActionResult GetHomeworkPlan(int id)
        {
            HomeworkPlan homeworkPlan = db.HomeworkPlans.Find(id);
            if (homeworkPlan == null)
            {
                return NotFound();
            }

            return Ok(homeworkPlan);
        }

        // PUT: api/HomeworkPlans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHomeworkPlan(int id, HomeworkPlan homeworkPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != homeworkPlan.HomeworkPlanID)
            {
                return BadRequest();
            }

            db.Entry(homeworkPlan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkPlanExists(id))
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

        // POST: api/HomeworkPlans
        [ResponseType(typeof(HomeworkPlan))]
        public IHttpActionResult PostHomeworkPlan(HomeworkPlan homeworkPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HomeworkPlans.Add(homeworkPlan);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = homeworkPlan.HomeworkPlanID }, homeworkPlan);
        }

        // DELETE: api/HomeworkPlans/5
        [ResponseType(typeof(HomeworkPlan))]
        public IHttpActionResult DeleteHomeworkPlan(int id)
        {
            HomeworkPlan homeworkPlan = db.HomeworkPlans.Find(id);
            if (homeworkPlan == null)
            {
                return NotFound();
            }

            db.HomeworkPlans.Remove(homeworkPlan);
            db.SaveChanges();

            return Ok(homeworkPlan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HomeworkPlanExists(int id)
        {
            return db.HomeworkPlans.Count(e => e.HomeworkPlanID == id) > 0;
        }
    }
}