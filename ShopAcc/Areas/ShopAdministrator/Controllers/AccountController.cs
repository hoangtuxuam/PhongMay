using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using ShopAcc.Areas.ShopAdministrator.Models;
using System.IO;

namespace ShopAcc.Areas.ShopAdministrator.Controllers
{
    public class AccountController : Controller
    {
        private ShopAccEntities1 db = new ShopAccEntities1();

        // GET: /ShopAdministrator/Account/
        [CustomAuthorize]
        public async Task<ActionResult> Index()
        {
            var lolaccs = db.LOLACCs.Include(l => l.RANK);
            return View(await lolaccs.ToListAsync());
        }

        // GET: /ShopAdministrator/Account/Details/5
        [CustomAuthorize]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOLACC lolacc = await db.LOLACCs.FindAsync(id);
            if (lolacc == null)
            {
                return HttpNotFound();
            }
            return View(lolacc);
        }

        // GET: /ShopAdministrator/Account/Create
        [CustomAuthorize]
        public ActionResult Create()
        {
            ViewBag.RANKID = new SelectList(db.RANKs, "ID", "TEN");
            ViewBag.SKINID = new SelectList(db.SKINs, "ID", "TEN");
            ViewBag.SKIN = new SelectList(db.SKINs, "ID", "TEN");
            //List<SelectListItem> lst
            return View();
        }

        // POST: /ShopAdministrator/Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "ID,TEN,GIA,MOTA,RANKID,USERNAME,PASSWORD")] LOLACC lolacc, IEnumerable<HttpPostedFileBase> ANHTUONG, IEnumerable<HttpPostedFileBase> ANHSKIN)
        {
            if (ModelState.IsValid)
            {
                lolacc.ID = Guid.NewGuid().ToString();
                #region Xử lý add ảnh
                if (ANHSKIN != null)
                {
                    List<string> lstFilePath = new List<string>();
                    foreach (var file in ANHSKIN)
                    {
                        // Some browsers send file names with full path. This needs to be stripped.
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = Guid.NewGuid().ToString() + file.FileName;
                        var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                        // The files are not actually saved in this demo
                        file.SaveAs(physicalPath);
                        lstFilePath.Add("~/Upload/" + fileName);
                        //Add vào db
                        ANH img = new ANH();
                        img.ID = Guid.NewGuid().ToString();
                        img.ACCID = lolacc.ID;
                        img.LINK = "Upload/" + fileName;
                        img.LOAI = 2;
                        db.ANHs.Add(img);
                    }
                }
                if (ANHTUONG != null)
                {
                    List<string> lstFilePath = new List<string>();
                    foreach (var file in ANHTUONG)
                    {
                        // Some browsers send file names with full path. This needs to be stripped.
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = Guid.NewGuid().ToString() + file.FileName;
                        var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                        // The files are not actually saved in this demo
                        file.SaveAs(physicalPath);
                        lstFilePath.Add("~/Upload/" + fileName);
                        //Add vào db
                        ANH img = new ANH();
                        img.ID = Guid.NewGuid().ToString();
                        img.ACCID = lolacc.ID;
                        img.LINK = "~/Upload/" + fileName;
                        img.LOAI = 1;
                        db.ANHs.Add(img);
                    }
                } 
                #endregion
                db.LOLACCs.Add(lolacc);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RANKID = new SelectList(db.RANKs, "ID", "TEN", lolacc.RANKID);
            return View(lolacc);
        }

        // GET: /ShopAdministrator/Account/Edit/5
        [CustomAuthorize]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOLACC lolacc = await db.LOLACCs.FindAsync(id);
            if (lolacc == null)
            {
                return HttpNotFound();
            }
            ViewBag.RANKID = new SelectList(db.RANKs, "ID", "TEN", lolacc.RANKID);
            List<string> lstSelectedSKINID = db.ACC_SKIN.Where(p => p.ACCID == id).Select(p => p.SKINID).ToList();
            MultiSelectList SelectSKIN = new MultiSelectList(db.SKINs, "ID", "TEN", lstSelectedSKINID.ToArray());
            string json = new JavaScriptSerializer().Serialize(Json(lstSelectedSKINID).Data);
            ViewBag.SKINID = SelectSKIN;
            ViewBag.SKINIdSelected = JsonConvert.SerializeObject(lstSelectedSKINID);
            lolacc.PASSWORD = "";
            return View(lolacc);
        }

        // POST: /ShopAdministrator/Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "ID,TEN,GIA,MOTA,RANKID,USERNAME,PASSWORD")] LOLACC lolacc, IEnumerable<HttpPostedFileBase> ANHTUONG, IEnumerable<HttpPostedFileBase> ANHSKIN)
        {
            if (ModelState.IsValid)
            {
                string SKINId = Request["SKINID"];
                List<string> lstSKINID = SKINId.Split(',').ToList();
                db.ACC_SKIN.RemoveRange(db.ACC_SKIN.Where(p => p.ACCID == lolacc.ID));
                foreach (string item in lstSKINID)
                {
                    ACC_SKIN obj = new ACC_SKIN();
                    obj.ID = Guid.NewGuid().ToString();
                    obj.ACCID = lolacc.ID;
                    obj.SKINID = item;
                    db.ACC_SKIN.Add(obj);
                }
                #region Xử lý add ảnh
                if (ANHSKIN != null)
                {
                    List<string> lstFilePath = new List<string>();
                    foreach (var file in ANHSKIN)
                    {
                        // Some browsers send file names with full path. This needs to be stripped.
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = Guid.NewGuid().ToString() + file.FileName;
                        var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                        // The files are not actually saved in this demo
                        file.SaveAs(physicalPath);
                        lstFilePath.Add("~/Upload/" + fileName);
                        //Add vào db
                        ANH img = new ANH();
                        img.ID = Guid.NewGuid().ToString();
                        img.ACCID = lolacc.ID;
                        img.LINK = "Upload/" + fileName;
                        img.LOAI = 2;
                        db.ANHs.Add(img);
                    }
                }
                if (ANHTUONG != null)
                {
                    List<string> lstFilePath = new List<string>();
                    foreach (var file in ANHTUONG)
                    {
                        // Some browsers send file names with full path. This needs to be stripped.
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = Guid.NewGuid().ToString() + file.FileName;
                        var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                        // The files are not actually saved in this demo
                        file.SaveAs(physicalPath);
                        lstFilePath.Add("~/Upload/" + fileName);
                        //Add vào db
                        ANH img = new ANH();
                        img.ID = Guid.NewGuid().ToString();
                        img.ACCID = lolacc.ID;
                        img.LINK = "Upload/" + fileName;
                        img.LOAI = 1;
                        db.ANHs.Add(img);
                    }
                }
                #endregion
                db.Entry(lolacc).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RANKID = new SelectList(db.RANKs, "ID", "TEN", lolacc.RANKID);
            return View(lolacc);
        }

        // GET: /ShopAdministrator/Account/Delete/5
        [CustomAuthorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOLACC lolacc = await db.LOLACCs.FindAsync(id);
            if (lolacc == null)
            {
                return HttpNotFound();
            }
            return View(lolacc);
        }

        // POST: /ShopAdministrator/Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            LOLACC lolacc = await db.LOLACCs.FindAsync(id);
            db.ACC_SKIN.RemoveRange(db.ACC_SKIN.Where(p => p.ACCID == lolacc.ID).ToList());
            db.ANHs.RemoveRange(db.ANHs.Where(p => p.ACCID == lolacc.ID).ToList());
            db.LOLACCs.Remove(lolacc);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
