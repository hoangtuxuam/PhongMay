using ShopAcc.Areas.ShopAdministrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopAcc.Areas.ShopAdministrator.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /ShopAdministrator/Home/
        [CustomAuthorize]
        public ActionResult Index()
        {
            return View();
        }
	}
}