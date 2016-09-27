using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models;
using ShopAcc.Areas.ShopAdministrator.Models;

namespace ShopAcc.Controllers
{
    public class TrangChuController : Controller
    {
        ShopAccEntities1 db = new ShopAccEntities1();
        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.ACCs = db.LOLACCs.Where(p => string.IsNullOrEmpty(p.THANHVIENID)).ToList();
            return View();
        }

        public ActionResult RankFilter(string id)
        {
            id = Request.Params["id"];
            ViewBag.ACCs = db.LOLACCs.Where(p => p.RANKID == id && string.IsNullOrEmpty(p.THANHVIENID)).ToList();
            return View();
        }

        public ActionResult SkinFilter(string id)
        {
            id = Request.Params["id"];
            ViewBag.ACCs = db.ACC_SKIN.Where(p => p.SKINID == id && string.IsNullOrEmpty(p.LOLACC.THANHVIENID)).Select(p => p.LOLACC).ToList();
            return View();
        }
        public ActionResult TienFilter(string id)
        {
            string tien = Request.Params["id"];
            int tu = Convert.ToInt32(tien.Split('-')[0]);
            int den = Convert.ToInt32(tien.Split('-')[1]);
            ViewBag.ACCs = db.LOLACCs.Where(p => p.GIA >= tu * 1000 && p.GIA < den * 1000 && string.IsNullOrEmpty(p.THANHVIENID)).ToList();
            return View();
        }

        public ActionResult NapThe()
        {
            CardInfo CardInfo = new CardInfo();
            List<SelectListItem> lstCARDTYPE = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(CardInfo.CardType)))
            {
                lstCARDTYPE.Add(new SelectListItem { Text = item.ToString(), Value = ((int)item).ToString() });
            }
            ViewBag.lstCARDTYPE = lstCARDTYPE;
            return View();
        }
        public JsonResult SubmitThe()
        {
            string message = "";
            AdminSession ses = new AdminSession();
            THANHVIEN tv = ses.getSession();
            if (tv == null)
            {
                message = "Bạn cần đăng nhập để sử dụng chức năng này.";
                return Json(message);
            }
            CardInfo card = new CardInfo();
            card.pin = Request.Params["pin"];
            card.seri = Request.Params["seri"];
            int cardtype = Convert.ToInt16(Request.Params["cardtype"]);
            if (string.IsNullOrEmpty(card.pin.Trim()) || string.IsNullOrEmpty(card.seri.Trim()))
            {
                message = "Mã thẻ hoặc số serial không thể trống.";
                return Json(message);
            }
            if (card.seri.Length < 8 || card.pin.Length < 8 || card.seri.Length > 20 || card.seri.Length > 20)
            {
                message = "Độ dài mã thẻ hoặc serial không đúng.";
                return Json(message);
            }

            const string merchant_id = "5795"; //API_ID lay trong tich hop website sau khi dang nhạp vippay.vn
            const string api_user = "b15cc45d784941c489c08168c5c39726"; //API_USERNAME lay trong tich hop website sau khi dang nhạp vippay.vn
            const string api_password = "6811e5b4661e4ffeaba2f85156e69c3b"; //API_PASSWORD lay trong tich hop website sau khi dang nhạp vippay.vn

            VippayCard pay = new VippayCard(merchant_id, api_user, api_password);
            string magd = Guid.NewGuid().ToString();
            string notes = magd + "-" + tv.FULLNAME + "-" + tv.ID + " nộp thẻ " + DateTime.Now.ToLongTimeString(); // Ghi chu ban tu them voi do dai khong qua 500 ky tu
            CardInfo info = pay.cardCharging(card.pin, card.seri, (CardInfo.CardType)(cardtype), notes);
            string mess;
            if (info.code == 0)
            {
                // Them logic sau khi nap the dung
                mess = string.Format("Nạp thẻ {0} thành công với mệnh giá {1}", Enum.GetName(typeof(CardInfo.CardType), info.cardtype), info.info_card);
                try
                {
                    int silk = 0;
                    int menhgia = int.Parse(info.info_card);

                    if (menhgia == 10000) { silk = 100; }
                    else if (menhgia == 20000) { silk = 200; }
                    else if (menhgia == 30000) { silk = 300; }
                    else if (menhgia == 50000) { silk = 500; }
                    else if (menhgia == 60000) { silk = 600; }
                    else if (menhgia == 70000) { silk = 700; }
                    else if (menhgia == 100000) { silk = 1000; }
                    else if (menhgia == 200000) { silk = 2000; }
                    else if (menhgia == 300000) { silk = 3000; }
                    else if (menhgia == 500000) { silk = 5000; }
                    else if (menhgia == 1000000) { silk = 10000; }

                    GIAODICH gd = new GIAODICH();
                    gd.ID = Guid.NewGuid().ToString();
                    gd.THANHVIENID = tv.ID;
                    gd.SOTIEN = silk;
                    gd.NOIDUNG = mess;
                    gd.THOIDIEM = DateTime.Now;
                    gd.MAGIAODICH = magd;
                    db.GIAODICHes.Add(gd);
                    THANHVIEN ThanhVien = db.THANHVIENs.Where(p => p.ID == tv.ID).FirstOrDefault();
                    if (ThanhVien.MONEY == null)
                    {
                        ThanhVien.MONEY = 0;
                    }
                    ThanhVien.MONEY += menhgia;
                    db.SaveChanges();
                    return Json(mess);
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                mess = string.Format("Vui lòng kiểm tra lại: {0}", info.msg);
                return Json(mess);
            }
            return Json("Đã có lỗi xảy ra. Liên hệ quản trị viên để biết thêm chi tiết!");
        }

        public ActionResult LichSuGiaoDich()
        {
            AdminSession ses = new AdminSession();
            THANHVIEN tv = ses.getSession();
            List<LOLACC> lstAcc = db.LOLACCs.Where(p => p.THANHVIENID == tv.ID).ToList();
            return View(lstAcc);
        }

        public JsonResult MuaAcc()
        {
            THANHVIEN tv = (THANHVIEN)System.Web.HttpContext.Current.Session["User"];
            if (tv == null)
                return Json("Bạn chưa đăng nhập. Đăng nhập để mua tài khoản");
            THANHVIEN ThanhVien = db.THANHVIENs.Where(p => p.ID == tv.ID).FirstOrDefault();
            string AccId = Request.Params["AccId"];
            LOLACC acc = db.LOLACCs.Where(p => p.ID == AccId).FirstOrDefault();
            if (!string.IsNullOrEmpty(acc.THANHVIENID))
                return Json("Acc đã có người mua!");
            if ((int)acc.GIA > (int)ThanhVien.MONEY)
                return Json("Số tiền bạn có không đủ!");
            ThanhVien.MONEY -= acc.GIA;
            acc.THANHVIENID = tv.ID;
            GIAODICH gd = new GIAODICH();
            gd.ID = Guid.NewGuid().ToString();
            gd.NOIDUNG = "Mua Acc " + acc.TEN;
            gd.MAGIAODICH = Guid.NewGuid().ToString();
            gd.THANHVIENID = tv.ID;
            gd.THOIDIEM = DateTime.Now;
            gd.SOTIEN = acc.GIA;
            db.GIAODICHes.Add(gd);
            db.SaveChanges();
            return Json("Mua Acc " + acc.TEN + " thành công!");
        }

        public ActionResult GetTinNhan()
        {
            AdminSession ses = new AdminSession();
            THANHVIEN tv = ses.getSession();
            List<TINNHAN> lstTINNHAN = db.TINNHANs.Where(p => p.HOITHOAI.THANHVIEN1.ID == tv.ID).OrderBy(p => p.NGAYGUI).ToList();
            return View(lstTINNHAN);
        }

        public ActionResult GiaoDich()
        {
            AdminSession ses = new AdminSession();
            THANHVIEN tv = ses.getSession();
            List<GIAODICH> lstGiaoDich = db.GIAODICHes.Where(p => p.THANHVIENID == tv.ID).ToList();
            return View(lstGiaoDich);
        }
    }
}