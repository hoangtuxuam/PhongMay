using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopAcc.Models;

namespace ShopAcc.Areas.ShopAdministrator.Models
{
    public class AdminSession
    {
        public THANHVIEN getSession()
        {
            return (THANHVIEN)System.Web.HttpContext.Current.Session["User"];
        }
    }
}