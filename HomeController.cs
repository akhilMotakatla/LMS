using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanMangSys.Models;


namespace LoanMangSys.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(tblAdmin objAdmin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(objAdmin.Password))
                {
                    TempData["ErrMsg"] = "Password field is required";
                }
                using ( LoanMangSysEntities db = new LoanMangSysEntities())
                {
                    var obj = db.tblAdmins.Where(a => a.AdminName.Equals(objAdmin.AdminName) && a.Password.Equals(objAdmin.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["AdminName"] = obj.AdminName.ToString();
                        return RedirectToAction("AdminDashBoard");
                    }
                  
                }
            }
            return View(objAdmin);
        }
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(tblUser objUser)
        {
            //if (ModelState.IsValid)
            //{
            using (LoanMangSysEntities db = new LoanMangSysEntities())
            {
                var obj = db.tblUsers.Where(a => a.UserName.Equals(objUser.UserName) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserName"] = obj.UserName.ToString();
                    Session["FirstName"] = obj.FirstName.ToString();
                    return RedirectToAction("UserDashBoard");
                }
                else
                {
                    TempData["Errmessage1"] = "Invalid credentials please enter valid credentials";
                }
            }
            return View(objUser);
        }
        public ActionResult UserDashboard()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return View("UserLogin");
            }
        }
        public ActionResult AdminDashBoard()
        {
            if (Session["AdminName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }
        public ActionResult AdminLogout()
        {
            Session.Clear();
            Session.Abandon();
            //Response.Redirect("AdminLogin");
            return View();
        }
       
        public ActionResult UserLogout()
        {
            Session.Clear();
            Session.Abandon();
           //return RedirectToAction("UserLogin");
           //Response.Redirect("UserLogin");
            return View();
        }

        public ActionResult NewUser()
        {
            return View();
        }

        public ActionResult HomePage()
        {
            return View();
        }
        public ActionResult EMICalculator()
        {
            return View();
        }

        public ActionResult EligibleCalculator()
        {
            double income = Convert.ToDouble(Request.Form["Income"]);
            double Loanamount = 60 * (0.6 * income);
            TempData["result"] = Loanamount;
            return View();
        }

        
    }
}