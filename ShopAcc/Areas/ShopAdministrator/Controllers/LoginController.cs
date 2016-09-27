using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Areas.ShopAdministrator.Models;
using System.Web.Security;
using ShopAcc.Models;

namespace ShopAcc.Areas.ShopAdministrator.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /ShopAdministrator/Login/
        ShopAccEntities1 db = new ShopAccEntities1();
        public ActionResult Index()
        {
            //var myvar = httpContext.Session["User"];
            Session.Remove("User");
            return View();
        }
        [HttpPost]
        public ActionResult CheckLogin(Login login)
        {
            ShopAcc.Models.ShopAccEntities1 db = new ShopAcc.Models.ShopAccEntities1();
            login = new Login();
            login.email = Request.Params["email"].ToString();
            login.password = Request.Params["password"].ToString();
            login.password = ShopAcc.Models.MaHoa.md5(login.password);
            ShopAcc.Models.THANHVIEN nv = new ShopAcc.Models.THANHVIEN();
            nv = (from p in db.THANHVIENs where p.EMAIL == login.email && p.PASSWORD == login.password select p).FirstOrDefault();
            if (nv != null)
            {
                Session["User"] = nv;
                FormsAuthentication.SetAuthCookie(nv.ID.ToString(), true);
                if (nv.QUYEN == null || nv.QUYEN == 0)
                    return Redirect("/TrangChu");
                else
                    return Redirect("/ShopAdministrator/Home");

            }
            return View("Index");
        }

        public JsonResult DangKy()
        {
            string email = Request.Params["email"].Trim();
            string password = Request.Params["password"].Trim();
            string fullname = Request.Params["FULLNAME"].Trim();
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(fullname))
            {
                THANHVIEN tv = new THANHVIEN();
                tv.ID = Guid.NewGuid().ToString();
                tv.PASSWORD = MaHoa.md5(password);
                tv.EMAIL = email;
                tv.FULLNAME = fullname;
                tv.MONEY = 0;
                db.THANHVIENs.Add(tv);
                return Json(1);
            }
            else
                return Json(0);
        }
    }
}