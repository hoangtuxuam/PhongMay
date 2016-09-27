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
using System.IO;
using ShopAcc.Areas.ShopAdministrator.Models;

namespace ShopAcc.Areas.ShopAdministrator.Controllers
{
    public class ThanhVienController : Controller
    {
        private ShopAccEntities1 db = new ShopAccEntities1();

        // GET: /ShopAdministrator/ThanhVien/
        [CustomAuthorize]
        public async Task<ActionResult> Index()
        {
            return View(await db.THANHVIENs.ToListAsync());
        }

        // GET: /ShopAdministrator/ThanhVien/Details/5
        [CustomAuthorize]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THANHVIEN thanhvien = await db.THANHVIENs.FindAsync(id);
            if (thanhvien == null)
            {
                return HttpNotFound();
            }
            return View(thanhvien);
        }

        // GET: /ShopAdministrator/ThanhVien/Create
        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ShopAdministrator/ThanhVien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "ID,EMAIL,PASSWORD,FULLNAME,AVATAR,MONEY")] THANHVIEN thanhvien, IEnumerable<HttpPostedFileBase> HINHANH)
        {
            if (ModelState.IsValid)
            {
                if (HINHANH != null)
                {
                    List<string> lstFilePath = new List<string>();
                    foreach (var file in HINHANH)
                    {
                        // Some browsers send file names with full path. This needs to be stripped.
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = Guid.NewGuid().ToString() + file.FileName;
                        var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                        // The files are not actually saved in this demo
                        file.SaveAs(physicalPath);
                        lstFilePath.Add("Upload/" + fileName);
                    }
                    if (lstFilePath.Count > 0)
                        thanhvien.AVATAR = string.Join(";", lstFilePath);
                }
                thanhvien.ID = Guid.NewGuid().ToString();
                thanhvien.PASSWORD = MaHoa.md5(thanhvien.PASSWORD);
                db.THANHVIENs.Add(thanhvien);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(thanhvien);
        }

        // GET: /ShopAdministrator/ThanhVien/Edit/5
        //[CustomAuthorize]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THANHVIEN thanhvien = await db.THANHVIENs.FindAsync(id);
            if (thanhvien == null)
            {
                return HttpNotFound();
            }
            thanhvien.PASSWORD = "";
            return View(thanhvien);
        }

        // POST: /ShopAdministrator/ThanhVien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CustomAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EMAIL,PASSWORD,FULLNAME,AVATAR,MONEY")] THANHVIEN thanhvien, IEnumerable<HttpPostedFileBase> HINHANH)
        {
            if (ModelState.IsValid)
            {
                if (HINHANH != null)
                {
                    List<string> lstFilePath = new List<string>();
                    foreach (var file in HINHANH)
                    {
                        // Some browsers send file names with full path. This needs to be stripped.
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = Guid.NewGuid().ToString() + file.FileName;
                        var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                        // The files are not actually saved in this demo
                        file.SaveAs(physicalPath);
                        lstFilePath.Add("Upload/" + fileName);
                    }
                    if (lstFilePath.Count > 0)
                        thanhvien.AVATAR = string.Join(";", lstFilePath);
                }
                if (!string.IsNullOrEmpty(thanhvien.PASSWORD))
                    thanhvien.PASSWORD = MaHoa.md5(thanhvien.PASSWORD);
                db.Entry(thanhvien).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(thanhvien);
        }

        // GET: /ShopAdministrator/ThanhVien/Delete/5
        [CustomAuthorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THANHVIEN thanhvien = await db.THANHVIENs.FindAsync(id);
            if (thanhvien == null)
            {
                return HttpNotFound();
            }
            return View(thanhvien);
        }

        // POST: /ShopAdministrator/ThanhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            THANHVIEN thanhvien = await db.THANHVIENs.FindAsync(id);
            db.THANHVIENs.Remove(thanhvien);
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
