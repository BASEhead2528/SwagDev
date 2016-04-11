using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SwagDevWeb.DAL;
using SwagDevWeb.Models;
using System.Diagnostics;
using System.IO;
using SwagDevWeb.Utilities;

namespace SwagDevWeb.Controllers
{

    public class SwagController : Controller
    {
        private SwagDBContext db = new SwagDBContext();

        // GET: Swag
        public async Task<ActionResult> Index()
        {
            return View(await db.Swags.ToListAsync());
        }

        // GET: Swag/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Swag swag = await db.Swags.FindAsync(id);
            if (swag == null)
            {
                return HttpNotFound();
            }
            return View(swag);
        }

        // GET: Swag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Swag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,UnitCost,Quantity,Description")] Swag swag, HttpPostedFileBase swagImage)
        {
            if (ModelState.IsValid)
            {
                if ((swagImage != null) && StaticMethods.isValidImageFile(swagImage))
                {
                    var fileName = Path.GetFileName(swagImage.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/SwagImages"), fileName);
                    swag.Image = "/Content/SwagImages/" + fileName;
                    swagImage.SaveAs(path);
                }
                db.Swags.Add(swag);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(swag);
        }

        // GET: Swag/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Swag swag = await db.Swags.FindAsync(id);
            if (swag == null)
            {
                return HttpNotFound();
            }
            return View(swag);
        }

        // POST: Swag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,UnitCost,Quantity,Image,Description")] Swag swag, HttpPostedFileBase swagImage)
        {
            if (ModelState.IsValid)
            {
                if ((swagImage != null) && StaticMethods.isValidImageFile(swagImage))
                {
                    string fileName = Path.GetFileName(swagImage.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/SwagImages"), fileName);

                    var oldImage = Path.Combine(Server.MapPath(swag.Image));

                    if (path != oldImage)
                    {
                        System.IO.File.Delete(oldImage);
                        swag.Image = "/Content/SwagImages/" + fileName;
                        swagImage.SaveAs(path);
                    }
                }

                db.Entry(swag).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(swag);
        }

        // GET: Swag/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Swag swag = await db.Swags.FindAsync(id);
            if (swag == null)
            {
                return HttpNotFound();
            }
            return View(swag);
        }

        // POST: Swag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Swag swag = await db.Swags.FindAsync(id);

            var imagePath = Path.Combine(Server.MapPath(swag.Image));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            db.Swags.Remove(swag);
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
