using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models;

namespace ShopAcc.Areas.ShopAdministrator.Controllers
{
    public class GiaoDichController : Controller
    {
        ShopAccEntities1 db = new ShopAccEntities1();
        //
        // GET: /ShopAdministrator/GiaoDich/
        public ActionResult Index()
        {
            List<GIAODICH> lstGiaoDich = db.GIAODICHes.OrderBy(p => p.THOIDIEM).ToList();
            return View(lstGiaoDich);
        }
	}
}