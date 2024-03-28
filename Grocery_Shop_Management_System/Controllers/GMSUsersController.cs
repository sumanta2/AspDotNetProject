using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Grocery_DAO;
using Grocery_Shop_Management_System.Models;


namespace Grocery_Shop_Management_System.Controllers
{
    public class GMSUsersController : Controller
    {
        private GMSDBContext db = new GMSDBContext();

        

        //GET: GMSUsers/Home
        public ActionResult Home()
        {
            return View();
        }

        // GET: GMSUsers
        //public ActionResult Index()
        //{
            
        //    return View(clsUser.ViewUser());
        //}

        // GET: GMSUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GMSUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userId,userName,password,userType,firstName,lastName,contactNumber,email")] user gMSUser)
        {
            if (ModelState.IsValid)
            {
                var userDetails = clsUser.FetchUserByName(gMSUser.userName);
                if((userDetails!= null) && (!userDetails.userName.Equals(gMSUser.userName)))
                {
                    ViewBag.Message = "UserName is Already Present";
                }
                else
                {
                    
                    clsUser.AddUser(gMSUser);
                }
                
                return RedirectToAction("LogedIn");
            }

            return View(gMSUser);
        }

        // GET: GMSUsers/LogedIn
        public ActionResult LogedIn()
        {
            return View("LogedIn");
        }

        // POST: GMSUsers/LogedIn
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogedIn([Bind(Include = "userName,password")] user gMSUser)
        {
            if (ModelState.IsValid)
            {
                if(clsUser.AuthenticateUser(gMSUser.userName,gMSUser.password))
                {
                    var userDetails = clsUser.FetchUserByName(gMSUser.userName);
                    Session["userId"] = userDetails.userId;
                    Session["userName"] = userDetails.userName;
                    Session["userType"] = userDetails.userType;
                    return RedirectToAction("ProductCards", "GMSProducts");
                }
                               
            }
            ViewBag.Message = "Wrong UserName or Password";
            return View("LogedIn");
        }

        // GET: GMSUsers/LogedOut
        public ActionResult LogedOut()
        {
            Session["userId"] = null;
            Session["userName"] = null;
            Session["userType"] = null;
            //return View("LogedIn");
            return RedirectToAction("LogedIn");
        }

        // GET: GMSUsers/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    user gMSUser = clsUser.FetchUser(id);
        //    if (gMSUser == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(gMSUser);
        //}

        //// POST: GMSUsers/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "userId,userName,password,userType,firstName,lastName,contactNumber,email")] GMSUser gMSUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        clsUser.UpdateUser(gMSUser.userId, gMSUser.userName, gMSUser.password, gMSUser.userType, gMSUser.firstName, gMSUser.lastName, gMSUser.contactNumber, gMSUser.email);
        //        return RedirectToAction("Index");
        //    }
        //    return View(gMSUser);
        //}
        

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
