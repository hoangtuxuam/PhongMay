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
    public class RankController : Controller
    {
        private ShopAccEntities1 db = new ShopAccEntities1();

        // GET: /ShopAdministrator/Rank/
        [CustomAuthorize]
        public async Task<ActionResult> Index()
        {
            return View(await db.RANKs.ToListAsync());
        }

        // GET: /ShopAdministrator/Rank/Details/5
        [CustomAuthorize]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RANK rank = await db.RANKs.FindAsync(id);
            if (rank == null)
            {
                return HttpNotFound();
            }
            return View(rank);
        }

        // GET: /ShopAdministrator/Rank/Create
        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ShopAdministrator/Rank/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "ID,TEN,HINHANH")] RANK rank, HttpPostedFileBase HINHANH)
        {
            if (ModelState.IsValid)
            {
                if (HINHANH != null)
                {
                    List<string> lstFilePath = new List<string>();

                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(HINHANH.FileName);
                    fileName = Guid.NewGuid().ToString() + HINHANH.FileName;
                    var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                    // The files are not actually saved in this demo
                    HINHANH.SaveAs(physicalPath);
                    lstFilePath.Add("~/Upload/" + fileName);
                    if (lstFilePath.Count > 0)
                        rank.HINHANH = string.Join(";", lstFilePath);
                }
                rank.ID = Guid.NewGuid().ToString();
                db.RANKs.Add(rank);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(rank);
        }

        // GET: /ShopAdministrator/Rank/Edit/5
        [CustomAuthorize]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RANK rank = await db.RANKs.FindAsync(id);
            if (rank == null)
            {
                return HttpNotFound();
            }
            return View(rank);
        }

        // POST: /ShopAdministrator/Rank/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "ID,TEN,HINHANH")] RANK rank, HttpPostedFileBase HINHANH)
        {
            if (ModelState.IsValid)
            {
                if (HINHANH != null)
                {
                    List<string> lstFilePath = new List<string>();

                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(HINHANH.FileName);
                    fileName = Guid.NewGuid().ToString() + HINHANH.FileName;
                    var physicalPath = Path.Combine(Server.MapPath("~/Upload"), fileName);

                    // The files are not actually saved in this demo
                    HINHANH.SaveAs(physicalPath);
                    lstFilePath.Add("~/Upload/" + fileName);
                    if (lstFilePath.Count > 0)
                        rank.HINHANH = string.Join(";", lstFilePath);
                }
                db.Entry(rank).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rank);
        }

        // GET: /ShopAdministrator/Rank/Delete/5
        [CustomAuthorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RANK rank = await db.RANKs.FindAsync(id);
            if (rank == null)
            {
                return HttpNotFound();
            }
            return View(rank);
        }

        // POST: /ShopAdministrator/Rank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            RANK rank = await db.RANKs.FindAsync(id);
            db.RANKs.Remove(rank);
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
