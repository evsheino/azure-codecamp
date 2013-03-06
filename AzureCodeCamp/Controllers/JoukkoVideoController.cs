using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureCodeCamp.Models;
using AzureCodeCamp.Utils;

namespace AzureCodeCamp.Controllers
{
    public class JoukkoVideoController : Controller
    {
        private JoukkoVideoDBContext db = new JoukkoVideoDBContext();

        //
        // GET: /JoukkoVideo/

        public ActionResult Index(int? page)
        {
            /**
            * Get videos ordered by timestamp (newest first).
            * The results are paginated.
            **/

            const int pageSize = 20;
            int pageNum = (page ?? 1);

            var videos = db.JoukkoVideos.OrderByDescending(jv => jv.timestamp);

            var paginator = new PaginatedList<JoukkoVideo>(videos, pageNum, pageSize);

            return View(paginator);
        }

        //
        // GET: /JoukkoVideo/Details/5

        public ActionResult Details(int id = 0)
        {
            JoukkoVideo joukkovideo = db.JoukkoVideos.Find(id);
            if (joukkovideo == null)
            {
                return HttpNotFound();
            }
            return View(joukkovideo);
        }

        //
        // GET: /JoukkoVideo/Create

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /JoukkoVideo/Create

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JoukkoVideo joukkovideo)
        {
            if (ModelState.IsValid)
            {
                db.JoukkoVideos.Add(joukkovideo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(joukkovideo);
        }

        //
        // GET: /JoukkoVideo/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            JoukkoVideo joukkovideo = db.JoukkoVideos.Find(id);
            if (joukkovideo == null)
            {
                return HttpNotFound();
            }
            return View(joukkovideo);
        }

        //
        // POST: /JoukkoVideo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JoukkoVideo joukkovideo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(joukkovideo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(joukkovideo);
        }

        //
        // GET: /JoukkoVideo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            JoukkoVideo joukkovideo = db.JoukkoVideos.Find(id);
            if (joukkovideo == null)
            {
                return HttpNotFound();
            }
            return View(joukkovideo);
        }

        //
        // POST: /JoukkoVideo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JoukkoVideo joukkovideo = db.JoukkoVideos.Find(id);
            db.JoukkoVideos.Remove(joukkovideo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}