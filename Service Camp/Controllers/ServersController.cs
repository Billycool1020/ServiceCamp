using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Service_Camp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Service_Camp.DTO;

namespace Service_Camp.Controllers
{
    [Authorize(Roles = "Trainee")]
    public class ServersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Servers
        public ActionResult Index()
        {
            
            return View(db.Servers.Where(x=>x.State=="Ready").ToList());
        }

        public ActionResult MyServer()
        {
            MyServerDTO dto = new MyServerDTO();
            dto.AvaliableServer = db.Servers.Where(x => x.State == "Ready").ToList();
            
            var user = db.UserServers.Find(User.Identity.Name);
            if (user != null)
            {
                dto.MyServer = user.Servers.ToList();
            }
            else
            {
                dto.MyServer = new List<Server>();
            }
            dto.ApplyRecord = db.ApplyRecords.Where(x => x.Applicant == User.Identity.Name).OrderByDescending(o => o.CreateDate).Take(10).ToList();


            return View(dto);
        }

        // GET: Servers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // GET: Servers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Servers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServerId,Detail,Name,IP")] Server server)
        {
            if (ModelState.IsValid)
            {
                server.State = "Ready";
                db.Servers.Add(server);
                db.SaveChanges();
                return RedirectToAction("MyServer");
            }

            return View(server);
        }

        // GET: Servers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServerId,Detail,Name,IP,State")] Server server)
        {
            if (ModelState.IsValid)
            {
                db.Entry(server).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Admin",null);
            }
            return View(server);
        }

        // GET: Servers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // POST: Servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Server server = db.Servers.Find(id);
            db.Servers.Remove(server);
            db.SaveChanges();
            return RedirectToAction("MyServer");
        }

        public ActionResult Apply(string Id)
        {
            ApplyRecord ApplyRecord = new ApplyRecord();
            ApplyRecord.Applicant = User.Identity.Name;
            ApplyRecord.CreateDate = DateTime.Now;
            ApplyRecord.ServerId = Id;
            ApplyRecord.States = "Active";
            db.ApplyRecords.Add(ApplyRecord);
            db.SaveChanges();

            return RedirectToAction("MyServer");
        }

        [HttpPost]
        public ActionResult Apply(string[] ServerId)
        {
            if (ServerId != null)
            {
                foreach (var id in ServerId)
                {
                    ApplyRecord ApplyRecord = new ApplyRecord();
                    ApplyRecord.Applicant = User.Identity.Name;
                    ApplyRecord.CreateDate = DateTime.Now;
                    ApplyRecord.ServerId = id;
                    ApplyRecord.States = "Active";
                    db.ApplyRecords.Add(ApplyRecord);
                }
                db.SaveChanges();
            }
            
            return RedirectToAction("MyServer");
        }

        
        [HttpPost]
        public ActionResult RetrunVM(string ServerId)
        {
            var user = db.UserServers.Find(User.Identity.Name);
            var server = db.Servers.Find(ServerId);
            user.Servers.Remove(server);
            ServerRentalRecord rentalRecord = db.ServerRentalRecords.Where(x => x.ServerId == ServerId && x.Renter == User.Identity.Name && x.IsActive == true).FirstOrDefault();
            rentalRecord.To = DateTime.Now;
            rentalRecord.IsActive = false;
            server.State = "Dirty";
            db.SaveChanges();
            return Json("ok");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
