using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPMVC01.Models;

namespace ASPMVC01.Controllers
{
    public class HomeController : Controller
    {
        dbProductEntities db = new dbProductEntities();

        // GET: Home
        public ActionResult Index()
        {
            var products = db.tProduct.ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string fid,string fname,decimal fprice,HttpPostedFileBase fImg)
        {
            string filename = "";
            if(fImg !=null)
            {
                if(fImg.ContentLength >0)
                {
                    filename = System.IO.Path.GetFileName(fImg.FileName);//取得檔案名稱
                    fImg.SaveAs(Server.MapPath("~/images/" + filename));
                }
            }
            tProduct newproduct = new tProduct();
            newproduct.fId = fid;
            newproduct.fName = fname;
            newproduct.fPrice = fprice;
            newproduct.fImg = filename;
            db.tProduct.Add(newproduct);
            db.SaveChanges();
            var products = db.tProduct.ToList();
            return View("Index", products);
        }

        public ActionResult Edit(string id)
        {
            var product = db.tProduct.Where(p => p.fId == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(string fid, string fname, decimal fprice, HttpPostedFileBase fImg, string oldImg)
        {
            string filename = "";
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    System.IO.File.Delete(Server.MapPath("~/images/" + oldImg));
                    filename = System.IO.Path.GetFileName(fImg.FileName);//取得檔案名稱
                    fImg.SaveAs(Server.MapPath("~/images/" + filename));
                }
            }
            if (filename == "")
                filename = oldImg;
            var product = db.tProduct.Where(p => p.fId == fid).FirstOrDefault();
            product.fId = fid;
            product.fName = fname;
            product.fPrice = fprice;
            product.fImg = filename;
            db.SaveChanges();
            var products = db.tProduct.ToList();
            return View("Index", products);
        }

        public ActionResult Delete(string id)
        {
            var product = db.tProduct.Where(p => p.fId == id).FirstOrDefault();
            string filename = product.fImg;
            System.IO.File.Delete(Server.MapPath("~/images/" + filename));
            db.tProduct.Remove(product);
            db.SaveChanges();
            var products = db.tProduct.ToList();
            return View("Index", products);
        }
    }
}