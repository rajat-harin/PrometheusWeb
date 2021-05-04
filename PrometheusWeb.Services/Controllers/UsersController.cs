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

namespace PrometheusWeb.Services.Controllers
{
    public class UsersController : ApiController
    {
        private PrometheusEntities db = new PrometheusEntities();

        // GET: api/Users
        public IQueryable<AdminUserModel> GetUsers()
        {
            return db.Users.Select(item => new AdminUserModel
            {
                UserID = item.UserID,
                Password = item.Password,
                Role = item.Role,
                SecurityQuestion = item.SecurityQuestion,
                SecurityAnswer = item.SecurityAnswer
            });
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            AdminUserModel User = new AdminUserModel
            {
                UserID = user.UserID,
                Password = user.Password,
                Role = user.Role,
                SecurityQuestion = user.SecurityQuestion,
                SecurityAnswer = user.SecurityAnswer
            };

            return Ok(User);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(string id, AdminUserModel adminModel)
        {
            User user = new User
            {
                UserID = adminModel.UserID,
                Password = adminModel.Password,
                Role = adminModel.Role,
                SecurityQuestion = adminModel.SecurityQuestion,
                SecurityAnswer = adminModel.SecurityAnswer
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(AdminUserModel adminModel)
        {
            User user = new User
            {
                UserID = adminModel.UserID,
                Password = adminModel.Password,
                Role = adminModel.Role,
                SecurityQuestion = adminModel.SecurityQuestion,
                SecurityAnswer = adminModel.SecurityAnswer
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}