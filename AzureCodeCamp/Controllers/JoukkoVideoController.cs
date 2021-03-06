﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureCodeCamp.Models;
using AzureCodeCamp.Utils;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using AzureCodeCamp.Filters;
using System.IO;

namespace AzureCodeCamp.Controllers
{
    [InitializeSimpleMembership]
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

            const int pageSize = 10;
            int pageNum = (page ?? 1);

            var videos = db.JoukkoVideos.OrderByDescending(jv => jv.timestamp);

            var paginator = new PaginatedList<JoukkoVideo>(videos, pageNum, pageSize);

            return View(paginator);
        }

        public ActionResult VideoList(PaginatedList<JoukkoVideo> videos)
        {
            return View(videos);
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

            ViewBag.categories = new SelectList(db.Category, "ID", "Name");
            return View();
        }

        //
        // POST: /JoukkoVideo/Create

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JoukkoVideo joukkovideo, HttpPostedFileBase file)
        {
            
            if (file != null && file.ContentLength > 0)
            {
                
                BlobStorage.uploadBlob(file, WebSecurity.CurrentUserId, joukkovideo.title, joukkovideo.categoryId);

                return RedirectToAction("Index");
            }

            ViewBag.categories = new SelectList(db.Category, "ID", "Name");
            ModelState.AddModelError("", "Please choose a file to upload");
            return View(joukkovideo);
        }

        //
        // GET: /JoukkoVideo/Edit/5

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.categories = db.Category.ToList();
            JoukkoVideo joukkovideo = db.JoukkoVideos.Find(id);
            if (joukkovideo == null)
            {
                return HttpNotFound();
            }
            return View(joukkovideo);
        }

        //
        // POST: /JoukkoVideo/Edit/5

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JoukkoVideo joukkovideo)
        {
            if (ModelState.IsValid)
            {
                if (joukkovideo.user.UserId == WebSecurity.CurrentUserId ||
                    Roles.GetRolesForUser().Contains("Admin"))
                {
                    db.Entry(joukkovideo).State = EntityState.Modified;
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Permission denied");
                }

            }
            return View(joukkovideo);
        }

        //
        // GET: /JoukkoVideo/Delete/5

        [Authorize(Roles="Admin")]
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

        [Authorize(Roles="Admin")]
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