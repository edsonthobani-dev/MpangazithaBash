using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MpangazithaBash.DAL;
using MpangazithaBash.Repository;

namespace MpangazithaBash.Controllers
{
    public class CategoriesController : Controller
    {

        public ActionResult Categories()
        {
            using (var uow = new GenericUnitOfWork())
            {
                var repo = uow.GetIRepositoryInstance<Tbl_Category>();
                List<Tbl_Category> categories = repo.GetAllRecords().ToList();
                return View(categories);
            }
        }
    }
}

