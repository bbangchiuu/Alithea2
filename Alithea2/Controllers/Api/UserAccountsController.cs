using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Alithea2.Models;

namespace Alithea2.Controllers.Api
{
    public class UserAccountsController : ApiController
    {
        private MyDbContext db = new MyDbContext();

        // GET: api/UserAccounts
        public IQueryable<UserAccount> GetUserAccounts()
        {
            return db.UserAccounts;
        }

        [ResponseType(typeof(UserAccount))]
        public IHttpActionResult GetUsername(string username)
        {
            Debug.WriteLine("dang chay username");
            UserAccount userAccounts = db.UserAccounts.FirstOrDefault(c => c.Username == username);
            if (userAccounts == null)
            {
                return NotFound();
            }

            return Ok(userAccounts);
        }

        [ResponseType(typeof(UserAccount))]
        public IHttpActionResult GetEmail(string email)
        {
            Debug.WriteLine("dang chay email");
            UserAccount userAccounts = db.UserAccounts.FirstOrDefault(c => c.Email == email);
            if (userAccounts == null)
            {
                return NotFound();
            }

            return Ok(userAccounts);
        }

        // GET: api/UserAccounts/5
        [ResponseType(typeof(UserAccount))]
        public IHttpActionResult GetUserAccount(int id)
        {
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return Ok(userAccount);
        }

        // PUT: api/UserAccounts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserAccount(int id, UserAccount userAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userAccount.UserID)
            {
                return BadRequest();
            }

            db.Entry(userAccount).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAccountExists(id))
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

        // POST: api/UserAccounts
        [ResponseType(typeof(UserAccount))]
        public IHttpActionResult PostUserAccount(UserAccount userAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserAccounts.Add(userAccount);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userAccount.UserID }, userAccount);
        }

        // DELETE: api/UserAccounts/5
        [ResponseType(typeof(UserAccount))]
        public IHttpActionResult DeleteUserAccount(int id)
        {
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return NotFound();
            }

            db.UserAccounts.Remove(userAccount);
            db.SaveChanges();

            return Ok(userAccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserAccountExists(int id)
        {
            return db.UserAccounts.Count(e => e.UserID == id) > 0;
        }
    }
}