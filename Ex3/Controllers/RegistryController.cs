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
using System.Security.Cryptography;
using System.Text;

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
        public IHttpActionResult GetRegistryModel(string UserName)
        {
            string[] arr = UserName.Split(' ');
            string usern = arr[0];
            string password = arr[1];
            RegistryModel player = db.RegistryModels.SingleOrDefault(user => user.UserName == usern);
            if (player == null)
            {
                return NotFound();
            }
            if (ComputeHash(password) != player.Password)
            {
                return NotFound();
            }

            return Ok(player);
        }

        // GET: api/Registry/SetRank/username/update/0
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
            registryModel.Password = ComputeHash(registryModel.Password);
            
            try
            {
                if (db.RegistryModels.Where(m => m.UserName == registryModel.UserName).Any() )
                {

                    throw new DbUpdateException();
                }
                db.RegistryModels.Add(registryModel);
                db.SaveChanges();

            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
                

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

        private string ComputeHash(string input)
        {
            SHA1 sha = SHA1.Create();
            byte[] buffer = Encoding.ASCII.GetBytes(input);
            byte[] hash = sha.ComputeHash(buffer);
            string hash64 = Convert.ToBase64String(hash);
            return hash64;
        }
    }
}