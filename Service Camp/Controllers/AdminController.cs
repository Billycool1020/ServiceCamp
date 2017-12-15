
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
            return View();
        }

        public ActionResult HandleApplication ()
        {
            var ApplicationList = db.ApplyRecords.Where(x => x.States == "Active");
            //var currentUser = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByName(ApplicationList.FirstOrDefault().Applicant);
            //currentUser.Server.Count();
            return View(ApplicationList);
        }

        public ActionResult Decline(int? id)
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
            record.States = "Decline";
            record.HandleDate = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("HandleApplication"); 
        }
    }
}