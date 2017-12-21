
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Service_Camp.DTO;
using Service_Camp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Service_Camp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            AdminDTO adminDTO = new AdminDTO();
            adminDTO.AllServers = db.Servers.ToList();
            adminDTO.ApplyRecords = db.ApplyRecords.Where(x => x.States == "Active").ToList();
            adminDTO.DirtyServers = db.Servers.Where(x => x.State == "Dirty").ToList();
            adminDTO.RentendServers = db.Servers.Where(x => x.State == "Rented").ToList();
            return View(adminDTO);
        }

        public ActionResult HandleApplication()
        {
            var ApplicationList = db.ApplyRecords.Where(x => x.States == "Active");
            //var currentUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByName(ApplicationList.FirstOrDefault().Applicant);
            //currentUser.Server.Count();
            return View(ApplicationList);
        }


        public ActionResult Clean(string id)
        {
            var server = db.Servers.Find(id);
            server.State = "Ready";
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Recapture(string id)
        {
            var server = db.Servers.Find(id);
            server.State = "Ready";

            var record = db.ServerRentalRecords.Where(x => x.IsActive == true && x.ServerId == id).FirstOrDefault();
            record.To = DateTime.Now;
            record.IsActive = false;
            var user = db.UserServers.Find(record.Renter);
            user.Servers.Remove(server);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Handle(int? id, string result)
        {
            if (id == null)
            {
                return RedirectToAction("HandleApplication");
            }
            ApplyRecord record = db.ApplyRecords.Find(id);
            if (record == null)
            {
                return RedirectToAction("HandleApplication");
            }
            record.Admin = User.Identity.Name;
            record.States = result;
            record.HandleDate = DateTime.Now;
            if (result == "Approve")
            {
                var server = db.Servers.Find(record.ServerId);
                var user = db.UserServers.Find(record.Applicant);
                if (user == null)
                {
                    user = new UserServer();
                    user.UserId = record.Applicant;
                    user.Servers = new List<Server>();
                    user.Servers.Add(server);
                    db.UserServers.Add(user);
                }
                else
                {
                    user.Servers.Add(server);
                }
                server.State = "Rented";

                ServerRentalRecord RentalRecord = new ServerRentalRecord();
                RentalRecord.ServerId = record.ServerId;
                RentalRecord.From = DateTime.Now;
                RentalRecord.IsActive = true;
                RentalRecord.Renter = User.Identity.Name;
                db.ServerRentalRecords.Add(RentalRecord);
            }

            db.SaveChanges();
            if (result == "Approve")
            {
                var OtherApply = db.ApplyRecords.Where(x => x.ServerId == record.ServerId && x.States == "Active").ToList();
                foreach (var other in OtherApply)
                {
                    other.States = "Decline";
                }
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}