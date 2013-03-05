using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureCodeCamp.Models;

namespace AzureCodeCamp.Controllers
{
    public class JoukkoVideoController : Controller
    {
        private JoukkoVideoDBContext db = new JoukkoVideoDBContext();

        //
        // GET: /JoukkoVideo/

        public ActionResult Index()
        {
            return View(db.JoukkoVideos.ToList());
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

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /JoukkoVideo/Create

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