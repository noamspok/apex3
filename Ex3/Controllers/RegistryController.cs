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
using Ex3.Models;

namespace Ex3.Controllers
{
    public class RegistryController : ApiController
    {
        private Ex3Context db = new Ex3Context();

        // GET: api/Registry
        public IQueryable<RegistryModel> GetRegistryModels()
        {
            return db.RegistryModels;
        }

        // GET: api/Registry/5
        [ActionName("GetUserName")]
        public IEnumerable<RegistryModel> GetRegistryModel(string UserName)
        {

            return db.RegistryModels.Where(m => m.UserName == UserName);
        }

        // GET: api/Registry/SetRank/username/update
        [ActionName("SetRank")]
        public IEnumerable<RegistryModel> GetRegistrModel(string Username,string update)
        {

            RegistryModel player = db.RegistryModels.SingleOrDefault(user => user.UserName == Username);
            if (update == "Win")
            {
                player.Wins += 1;
            }
            else { player.Loses+= 1; }
            db.SaveChanges();
            return db.RegistryModels.Where(m => m.UserName == Username);
        }

        // PUT: api/Registry/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRegistryModel(int id, RegistryModel registryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != registryModel.Id)
            {
                return BadRequest();
            }

            db.Entry(registryModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistryModelExists(id))
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

        // POST: api/Registry
        [ResponseType(typeof(RegistryModel))]
        public IHttpActionResult PostRegistryModel(RegistryModel registryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(db.RegistryModels.Where(m => m.UserName == registryModel.UserName) != null)
            {

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Conflict,new Exception("UserName allready exists!")));
            }
            db.RegistryModels.Add(registryModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = registryModel.Id }, registryModel);
        }

        // DELETE: api/Registry/5
        [ResponseType(typeof(RegistryModel))]
        public IHttpActionResult DeleteRegistryModel(int id)
        {
            RegistryModel registryModel = db.RegistryModels.Find(id);
            if (registryModel == null)
            {
                return NotFound();
            }

            db.RegistryModels.Remove(registryModel);
            db.SaveChanges();

            return Ok(registryModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegistryModelExists(int id)
        {
            return db.RegistryModels.Count(e => e.Id == id) > 0;
        }
    }
}