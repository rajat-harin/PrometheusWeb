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
using PrometheusWeb.Services.Services;

namespace PrometheusWeb.Services.Controllers
{
    public class AssignmentsController : ApiController
    {
        private IAssignmentService _assignmentService = null;
        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        // GET: api/Assignments
        public IQueryable<AssignmentUserModel> GetAssignments()
        {
            return _assignmentService.GetAssignments();
        }

        // GET: api/Assignments/5
        [ResponseType(typeof(Assignment))]
        public IHttpActionResult GetAssignment(int id)
        {
            AssignmentUserModel assignment = _assignmentService.GetAssignment(id);
            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

        // PUT: api/Assignments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAssignment(int id, AssignmentUserModel assignment)
        {
            bool result;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                result = _assignmentService.UpdateAssignment(id, assignment);
            }
            catch (Exception)
            {
                if (!_assignmentService.IsAssignmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Assignments
        [ResponseType(typeof(Assignment))]
        public IHttpActionResult PostAssignment(AssignmentUserModel assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _assignmentService.AddAssignment(assignment);
            if (result)
                return CreatedAtRoute("DefaultApi", new { id = assignment.AssignmentID }, assignment);
            else
                return StatusCode(HttpStatusCode.InternalServerError);
        }

        // DELETE: api/Assignments/5
        [ResponseType(typeof(Assignment))]
        public IHttpActionResult DeleteAssignment(int id)
        {
            try
            {
                AssignmentUserModel assignment = _assignmentService.DeleteAssignment(id);
                if(assignment != null)
                {
                    return Ok(assignment);
                }
                else
                {
                    return StatusCode(HttpStatusCode.NotFound);
                }
            }
            catch(Exception)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            
        }

        protected override void Dispose(bool disposing)
        {
           
            base.Dispose(disposing);
        }

    }
}