using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoanMangSys.Models;

namespace LoanMangSys.Controllers
{
    public class tblLoansController : Controller
    {
        private LoanMangSysEntities db = new LoanMangSysEntities();

        // GET: tblLoans
        public ActionResult Index()
        {

            var tblLoans = db.tblLoans.Include(t => t.tblUser);
            if (Session["AdminName"] != null && Session["UserName"] is null)
            {
                return View(tblLoans.ToList());
            }
            else if (Session["AdminName"] is null && Session["UserName"] != null)
            {
                string x = Session["UserName"].ToString();
                return View(tblLoans.Where(a => a.UserName == x));
            }
            else
            {
                return View(db.tblLoans.ToList());
            }
        }

        // GET: tblLoans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLoan tblLoan = db.tblLoans.Where(x => x.ApplicationNo == id).FirstOrDefault();

            if (tblLoan == null)
            {
                return HttpNotFound();
            }
            return View(tblLoan);
        }

        // GET: tblLoans/Create
        public ActionResult Create()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("UserLogin");
            }
            string x = Session["UserName"].ToString();
            ViewBag.UserName = Session["UserName"].ToString();
            //tblUser obj = db.tblUsers.Find(x);
            return View();
        }

        // POST: tblLoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicationNo,PropertyDetails,LoanAmount,LoanTenure,EMI")] tblLoan tblLoan)
        {
            tblLoan.LoanStatus = "Pending";
            tblLoan.ApplicationDate = System.DateTime.Now;
            tblLoan.UserName = Session["UserName"].ToString();
            if (ModelState.IsValid)
            {
                db.tblLoans.Add(tblLoan);
                db.SaveChanges();
                return RedirectToAction("UserDashboard","Home");
            }

            ViewBag.UserName = new SelectList(db.tblUsers, "UserName", "FirstName", tblLoan.UserName);
            return View(tblLoan);
        }

        // GET: tblLoans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLoan tblLoan = db.tblLoans.Find(id);
            if (tblLoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserName = new SelectList(db.tblUsers, "UserName", "FirstName", tblLoan.UserName);
            return View(tblLoan);
        }

        // POST: tblLoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationNo,UserName,PropertyDetails,ApplicationDate,LoanAmount,LoanTenure,EMI,LoanStatus")] tblLoan tblLoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblLoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserName = new SelectList(db.tblUsers, "UserName", "FirstName", tblLoan.UserName);
            return View(tblLoan);
        }

        // GET: tblLoans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLoan tblLoan = db.tblLoans.Find(id);
            if (tblLoan == null)
            {
                return HttpNotFound();
            }
            return View(tblLoan);
        }

        // POST: tblLoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblLoan tblLoan = db.tblLoans.Find(id);
            db.tblLoans.Remove(tblLoan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
        public ActionResult PendingLoans()
        {
            string strUser = "";
            if (Session["UserName"] != null)
               strUser  = Session["UserName"].ToString();

            if (Session["AdminName"] != null)
            {
                var model = from r in db.tblLoans where r.LoanStatus == "Pending" select r;
                return View(model.ToList());
            }
            else if (Session["UserName"] != null)
            {
                var model = from r in db.tblLoans where r.UserName== strUser && r.LoanStatus == "Pending"  select r;
                return View(model.ToList());

            }
            return View();
        }

        [HttpPost, ActionName("PendingLoans")]
        [ValidateAntiForgeryToken]
        public ActionResult PendingLoansView()
        {
            return View();
        }
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLoan tblLoan = db.tblLoans.Find(id);
            if (tblLoan == null)
            {
                return HttpNotFound();
            }
            tblLoan.LoanStatus = "Approved";
            db.Entry(tblLoan).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLoan tblLoan = db.tblLoans.Find(id);
            if (tblLoan == null)
            {
                return HttpNotFound();
            }
            tblLoan.LoanStatus = "Rejected";
            db.Entry(tblLoan).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

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
