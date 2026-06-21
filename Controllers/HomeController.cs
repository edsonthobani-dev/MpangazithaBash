using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MpangazithaBash.Models.Home;
using PagedList;

namespace MpangazithaBash.Controllers
{
    public class HomeController: Controller
    {
        public ActionResult Index(string search, int page = 1)
        {
            ViewBag.Search = search;
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search, page));
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page";
            return View();
        }
    }
}