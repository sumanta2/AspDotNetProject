﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grocery_Shop_Management_System.Models;
using Grocery_DAO;

namespace Grocery_Shop_Management_System.Controllers
{
    public class GMSOrdersController : Controller
    {

        // GET: GMSOrders
        public ActionResult Index()
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("LogedIn", "GMSUsers");
            }
            int userId = (int)Session["userId"];
            string userType=(String)Session["userType"];
            return View(clsOrder.ViewOrder(userId,userType));
        }



        // GET: GMSOrders/Create
        //public ActionResult Create()
        //{
        //    ViewBag.userId = new SelectList(clsOrder.GetCustomer1(), "userId", "firstName");
        //    ViewBag.productId = new SelectList(clsOrder.GetProduct(), "productId", "productName");
        //    return View();
        //}

        // POST: GMSOrders/Create
        // in productDetails page when user press purchases then a order is generated by Details post method at productController
        //  ---it generally i used when tempurary insert data for testing purpose
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "orderId,userId,productId,productName,quantity,total")] order gMSOrder)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        clsOrder.AddOrder(gMSOrder);
        //        return RedirectToAction("Index");
        //    }

        //    return View("Index");
        //}


        //it use to generate bill page after purchases product
        public ActionResult Details()
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("LogedIn", "GMSUsers");
            }
            int orderId = (int)TempData["orderId"];
            if(orderId==null)
            {
                return RedirectToAction("ProductCards","GMSProducts");
            }
            order ord = clsOrder.FetchOrder(orderId);
            return View(ord);
        }


        // GET: GMSOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("LogedIn", "GMSUsers");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ordrModel = clsOrder.FetchOrder(id);
            if (ordrModel == null)
            {
                return HttpNotFound();
            }
            return View(ordrModel);
        }

        // POST: GMSOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            clsOrder.DeleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}
