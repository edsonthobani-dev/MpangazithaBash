using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MpangazithaBash.DAL;

namespace MpangazithaBash.Models.Home
{
    public class item
    {
        public Tbl_Product tbl_product { get; set; }

        public int Quantity { get; set; }

    }
}