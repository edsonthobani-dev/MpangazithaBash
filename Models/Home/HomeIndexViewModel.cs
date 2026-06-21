using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using MpangazithaBash.DAL;
using MpangazithaBash.Repository;

namespace MpangazithaBash.Models.Home
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        dbMyOnlineShoppingEntities context = new dbMyOnlineShoppingEntities();
        public IPagedList<Tbl_Product> listofProducts { get; set; }

        public HomeIndexViewModel CreateModel(string search, int page = 1)
        {
            int pageSize = 8;

            if (!string.IsNullOrEmpty(search))
            {
                IEnumerable<Tbl_Product> data = _unitOfWork
                    .GetIRepositoryInstance<Tbl_Product>()
                    .GetResultBySqlprocedure("exec GetBySearch @search",
                        new SqlParameter("@search", search));

                return new HomeIndexViewModel()
                {
                    listofProducts = data.ToPagedList(page, pageSize)
                };
            }
            else
            {
                return new HomeIndexViewModel()
                {
                    listofProducts = _unitOfWork.GetIRepositoryInstance<Tbl_Product>()
                        .GetAllRecords()
                        .Where(i => i.IsDelete == false)
                        .ToPagedList(page, pageSize)
                };
            }
        }
    }
    }

    