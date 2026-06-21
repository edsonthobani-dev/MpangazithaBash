using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MpangazithaBash.DAL;
using MpangazithaBash.Models;
using MpangazithaBash.Repository;
using Newtonsoft.Json;

namespace MpangazithaBash.Controllers
{
    public class ProductController : Controller
    {
        public GenericUnitOfWork _unitofWork = new GenericUnitOfWork();

        // =================== CATEGORIES ===================
        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitofWork.GetIRepositoryInstance<Tbl_Category>().GetAllRecords();
            foreach(var item in cat)
            {
                list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });

            }
            return list;
        }
        public ActionResult Categories()
        {
            List<Tbl_Category> allcategories = _unitofWork.GetIRepositoryInstance<Tbl_Category>()
                .GetAllRecordsIQueryable()
                .Where(i => i.isDelete == false)
                .ToList();
            return View(allcategories);
        }

        public ActionResult UpdateCategory(int? CategoryId)
        {
            CategoryDetail cd;
            if (CategoryId != null && CategoryId != 0)
            {
                var category = _unitofWork.GetIRepositoryInstance<Tbl_Category>()
                    .GetFirstorDefault(CategoryId.Value);

                cd = JsonConvert.DeserializeObject<CategoryDetail>(
                    JsonConvert.SerializeObject(category, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
            }
            else
            {
                cd = new CategoryDetail();
            }
            return View("UpdateCategory", cd);
        }

        public ActionResult AddCategory()
        {
            return UpdateCategory(0);
        }
        [HttpPost]
        public ActionResult CategorySave(Tbl_Category tbl_category)
        {
            if (tbl_category.CategoryId == 0)
            {
                tbl_category.isDelete = false;
                _unitofWork.GetIRepositoryInstance<Tbl_Category>().Add(tbl_category);
            }
            else
            {
                _unitofWork.GetIRepositoryInstance<Tbl_Category>().Update(tbl_category);
            }
            return RedirectToAction("Categories");
        }


        // =================== PRODUCTS ===================
        
        public ActionResult Product()
        {
            List<Tbl_Product> allproducts = _unitofWork.GetIRepositoryInstance<Tbl_Product>()
                .GetAllRecordsIQueryable()
                .Where(i => i.IsDelete == false)
                .ToList();
            return View(allproducts);
        }
       
        public ActionResult ProductAdd()
        {
            ViewBag.CategoryList = GetCategory();
            ViewBag.Categories = _unitofWork.GetIRepositoryInstance<Tbl_Category>()
                .GetAllRecordsIQueryable()
                .Where(i => i.isDelete == false)
                .ToList();
           
            return View(new ProductDetail());
        }
        
        public ActionResult ProductEdit(int? ProductId)
        {
            ProductDetail pd;
            if (ProductId != null && ProductId != 0)
            {
                var product = _unitofWork.GetIRepositoryInstance<Tbl_Product>()
                    .GetFirstorDefault(ProductId.Value);

                pd = JsonConvert.DeserializeObject<ProductDetail>(
                    JsonConvert.SerializeObject(product, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
            }
            else
            {
                pd = new ProductDetail();
            }
            ViewBag.CategoryList = GetCategory();
            ViewBag.Categories = _unitofWork.GetIRepositoryInstance<Tbl_Category>()
                .GetAllRecordsIQueryable()
                .Where(i => i.isDelete == false)
                .ToList();

            return View("ProductEdit", pd);
        }

        [HttpPost]
        public ActionResult ProductSave(Tbl_Product tbl_product, HttpPostedFileBase ProductImage)
        {
            if (ProductImage != null && ProductImage.ContentLength > 0)
            {
                string fileName = Path.GetFileName(ProductImage.FileName);
                string path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                // Create Images folder if it doesn't exist
                if (!Directory.Exists(Server.MapPath("~/Images/")))
                    Directory.CreateDirectory(Server.MapPath("~/Images/"));
                ProductImage.SaveAs(path);
                tbl_product.ProductImage = fileName;
            }
            else
            {
                tbl_product.ProductImage = "default.jpg"; // ✅ default if no image
            }

            tbl_product.CreatedDate = DateTime.Now;
            tbl_product.ModifiedDate = DateTime.Now;
            tbl_product.IsDelete = false;

            _unitofWork.GetIRepositoryInstance<Tbl_Product>().Add(tbl_product);
            return RedirectToAction("Product");
        }

        [HttpPost]
        public ActionResult ProductUpdate(Tbl_Product tbl_product, HttpPostedFileBase ProductImage)
        {
            if (ProductImage != null && ProductImage.ContentLength > 0)
            {
                string fileName = Path.GetFileName(ProductImage.FileName);
                string path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                // Create Images folder if it doesn't exist
                if (!Directory.Exists(Server.MapPath("~/Images/")))
                    Directory.CreateDirectory(Server.MapPath("~/Images/"));
                ProductImage.SaveAs(path);
                tbl_product.ProductImage = fileName;
            }
            else
            {
                // ✅ Use existing image from hidden field instead of querying DB
                tbl_product.ProductImage = Request.Form["ProductImage"];

                // ✅ If still null use default
                if (string.IsNullOrEmpty(tbl_product.ProductImage))
                    tbl_product.ProductImage = "default.jpg";
            }

            tbl_product.ModifiedDate = DateTime.Now;
            _unitofWork.GetIRepositoryInstance<Tbl_Product>().Update(tbl_product);
            return RedirectToAction("Product");
        }
    }
}