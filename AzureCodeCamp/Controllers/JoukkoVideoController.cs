using System;
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
        public ActionResult Create(JoukkoVideo joukkovideo, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                /* On niin shaibaa et tallennetaan välissä servulle, josta sitten siirrellään media serviceen*/
                string fn = Path.GetFileName(file.FileName);
                string SaveLocation = Server.MapPath("~\\tmpData") + "\\" + fn;
                file.SaveAs(SaveLocation);
                
                var addedFile = MediaServices.createAsset(SaveLocation);
                var blobId = MediaServices.encodeAsset(addedFile.Id);
                MediaServices.DeleteAsset(addedFile);


                var db1 = new UsersContext();
                joukkovideo.path = MediaServices.GetStreamingURL(blobId);
                var user = db1.UserProfiles.Single(u => u.UserId == WebSecurity.CurrentUserId);
                joukkovideo.user = user;
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