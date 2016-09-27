using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models;

namespace ShopAcc.Areas.ShopAdministrator.Models
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            THANHVIEN tv = (THANHVIEN)httpContext.Session["User"];
            return tv != null && tv.QUYEN == 1;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/ShopAdministrator/Login");
        }
    }
}