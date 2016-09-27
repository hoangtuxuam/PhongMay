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
    public class SkinController : Controller
    {
        private ShopAccEntities1 db = new ShopAccEntities1();

        // GET: /ShopAdministrator/Skin/
        [CustomAuthorize]
        public async Task<ActionResult> Index()
        {
            return View(await db.SKINs.ToListAsync());
        }

        // GET: /ShopAdministrator/Skin/Details/5
        [CustomAuthorize]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKIN skin = await db.SKINs.FindAsync(id);
            if (skin == null)
            {
                return HttpNotFound();
            }
            return View(skin);
        }

        // GET: /ShopAdministrator/Skin/Create
        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ShopAdministrator/Skin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "ID,TEN,HINHANH")] SKIN skin, IEnumerable<HttpPostedFileBase> HINHANH)
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
                        skin.HINHANH = string.Join(";", lstFilePath);
                }
                skin.ID = Guid.NewGuid().ToString();
                db.SKINs.Add(skin);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(skin);
        }

        // GET: /ShopAdministrator/Skin/Edit/5
        [CustomAuthorize]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKIN skin = await db.SKINs.FindAsync(id);
            if (skin == null)
            {
                return HttpNotFound();
            }
            return View(skin);
        }

        // POST: /ShopAdministrator/Skin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "ID,TEN,HINHANH")] SKIN skin, IEnumerable<HttpPostedFileBase> HINHANH)
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
                        skin.HINHANH = string.Join(";", lstFilePath);
                }
                db.Entry(skin).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(skin);
        }

        // GET: /ShopAdministrator/Skin/Delete/5
        [CustomAuthorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKIN skin = await db.SKINs.FindAsync(id);
            if (skin == null)
            {
                return HttpNotFound();
            }
            return View(skin);
        }

        // POST: /ShopAdministrator/Skin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            SKIN skin = await db.SKINs.FindAsync(id);
            db.SKINs.Remove(skin);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [CustomAuthorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<HttpPostedFileBase> files)
        {
            return
                from a in files
                where a != null
                select string.Format("{0} ({1} bytes)", Path.GetFileName(a.FileName), a.ContentLength);
        }
    }
}
