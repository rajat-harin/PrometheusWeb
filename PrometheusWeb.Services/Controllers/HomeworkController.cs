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
    public class HomeworkController : ApiController
    {
        private PrometheusEntities db = new PrometheusEntities();

        // GET: api/Homework
        public IQueryable<Homework> GetHomework()
        {
            return db.Homework;
        }

        // GET: api/Homework/5
        [ResponseType(typeof(Homework))]
        public IHttpActionResult GetHomework(int id)
        {
            Homework homework = db.Homework.Find(id);
            if (homework == null)
            {
                return NotFound();
            }

            return Ok(homework);
        }

        // PUT: api/Homework/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHomework(int id, Homework homework)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != homework.HomeWorkID)
            {
                return BadRequest();
            }

            db.Entry(homework).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkExists(id))
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

        // POST: api/Homework
        [ResponseType(typeof(Homework))]
        public IHttpActionResult PostHomework(Homework homework)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Homework.Add(homework);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (HomeworkExists(homework.HomeWorkID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = homework.HomeWorkID }, homework);
        }

        // DELETE: api/Homework/5
        [ResponseType(typeof(Homework))]
        public IHttpActionResult DeleteHomework(int id)
        {
            Homework homework = db.Homework.Find(id);
            if (homework == null)
            {
                return NotFound();
            }

            db.Homework.Remove(homework);
            db.SaveChanges();

            return Ok(homework);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HomeworkExists(int id)
        {
            return db.Homework.Count(e => e.HomeWorkID == id) > 0;
        }
    }
}