using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grocery_Shop_Management_System.Models;
using Grocery_DAO;
using System.IO;

namespace Grocery_Shop_Management_System.Controllers
{
    public class GMSProductsController : Controller
    {
        // GET: GMSProducts
        public ActionResult Index()
        {
            int? id = (int)Session["userId"];
            if(id==null)
            {
                ViewBag.Message = "Session is Expired";
                return RedirectToAction("LogedIn","GMSUsers");
            }

            List<product> products = clsProduct.ViewProductById((int)id);
            List<string> images = new List<string>();
            foreach(var x in products)
            {
                string strBase64Data = Convert.ToBase64String(x.image_name);
                string imgDataUrl = string.Format("data:;base64,{0}", strBase64Data);
                images.Add(imgDataUrl);
                
            }
            ViewBag.ImageData = images;

            return View(products);
        }
        
        // GET: GMSProducts/ProductCards  it return list of product after user logedin
        public ActionResult ProductCards()
        {
            if (Session["userId"]== null)
            {
                ViewBag.Message = "Please logedIn for display Product";
                return RedirectToAction("LogedIn","GMSUsers");
            }
            List<product> products = clsProduct.ViewProduct();
            List<string> images = new List<string>();
            foreach (var x in products)
            {
                string strBase64Data = Convert.ToBase64String(x.image_name);
                string imgDataUrl = string.Format("data:;base64,{0}", strBase64Data);
                images.Add(imgDataUrl);

            }
            ViewBag.ImageData = images;

            return View(clsProduct.ViewProduct());
        }

        // GET: GMSProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["userId"] == null)
            {
                ViewBag.Message = "Please LogedIn for display Detail";
                return RedirectToAction("LogedIn", "GMSUsers");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productModel = clsProduct.FetchProduct(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }

            
                string strBase64Data = Convert.ToBase64String(productModel.image_name);
                string imgDataUrl = string.Format("data:;base64,{0}", strBase64Data);
                

            
            ViewBag.ImageData = imgDataUrl;
            return View(productModel);
        }


        //after order any item from product details page this method call it 
        [HttpPost]
        public ActionResult Details([Bind(Include = "productId,price,stockQuantity,orderQuantity")] product gMSProduct)
        {
            //here i do order confirmation operation here i need to to decrease stoke after each product and if stock is less than 1 then
            //   not order will be performed
            if((int)gMSProduct.orderQuantity> gMSProduct.stockQuantity)
            {
                product prod= clsProduct.FetchProduct(gMSProduct.productId);
                return View(gMSProduct);
            }
            //this below method decrease the quantity of product in database after order
            //clsProduct.UpdateProductQuantity(gMSProduct.productId,(int)gMSProduct.orderQuantity);
            order ord=new order();
            ord.userId = (int)Session["userId"];
            ord.productId = gMSProduct.productId;
            ord.quantity = (int)gMSProduct.orderQuantity;
            var totalValue = (int)gMSProduct.orderQuantity * gMSProduct.price;
            ord.total = totalValue;
            int orid=clsOrder.AddOrder(ord);
            clsProduct.UpdateProductQuantity(gMSProduct.productId, (int)gMSProduct.orderQuantity);

            TempData["orderId"] = orid;
            return RedirectToAction("Details", "GMSOrders");
        }
        // GET: GMSProducts/Create
        public ActionResult Create()
        {
            if (Session["userId"] == null)
            {
                ViewBag.Message = "Please logedIn for add product";
                return RedirectToAction("LogedIn", "GMSUsers");
            }
            //ViewBag.userId = new SelectList(clsProduct.GetSeller(), "userId", "firstName");
            return View();
        }

        // POST: GMSProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "productId,productName,description,price,stockQuantity,image_name")] product gMSProduct, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if(Session["userId"]==null)
                {
                    ViewBag.Message = "Session is Expired";
                    RedirectToAction("LogedIn","GMSUsers");
                }
                gMSProduct.userId = (int)Session["userId"];
                
                if(upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        gMSProduct.image_name = reader.ReadBytes(upload.ContentLength);
                    }
                }
                clsProduct.AddProduct(gMSProduct);
                return RedirectToAction("Index");
            }

            //ViewBag.userId = new SelectList(clsProduct.GetSeller(), "userId", "firstName", gMSProduct.userId);
            return View(gMSProduct);
        }

        // GET: GMSProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["userId"] == null)
            {
                ViewBag.Message = "Please LogedIn for Edit product";
                return RedirectToAction("LogedIn", "GMSUsers");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var prodModel = clsProduct.FetchProduct(id);
            if (prodModel == null)
            {
                return HttpNotFound();
            }
            //ViewBag.userId = new SelectList(clsProduct.GetSeller(), "userId", "firstName", prodModel.userId);
            return View(prodModel);
        }

        // POST: GMSProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productId,userId,productName,description,price,stockQuantity,image_name")] product gMSProduct, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        gMSProduct.image_name = reader.ReadBytes(upload.ContentLength);
                    }
                }
                clsProduct.UpdateProduct(gMSProduct.productId,gMSProduct.productName, gMSProduct.description, gMSProduct.price.ToString(), gMSProduct.stockQuantity.ToString(), gMSProduct.image_name);
                return RedirectToAction("Index");
            }
            //ViewBag.userId = new SelectList(clsProduct.GetSeller(), "userId", "firstName", gMSProduct.userId);
            return View(gMSProduct);
        }

        // GET: GMSProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["userId"] == null && (string)Session["userType"] !="seller")
            {
                ViewBag.Message = "Please logedIn as seller";
                return RedirectToAction("LogedIn", "GMSUsers");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productModel = clsProduct.FetchProduct(id);
            if (productModel == null)
            {
                return HttpNotFound();
            }


            string strBase64Data = Convert.ToBase64String(productModel.image_name);
            string imgDataUrl = string.Format("data:;base64,{0}", strBase64Data);



            ViewBag.ImageData = imgDataUrl;
            return View(productModel);
        }

        // POST: GMSProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            clsProduct.DeleteProduct(id);
            return RedirectToAction("Index");
        }

    }
}
