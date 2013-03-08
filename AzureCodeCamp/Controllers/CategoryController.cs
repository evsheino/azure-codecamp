using AzureCodeCamp.Models;
using AzureCodeCamp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureCodeCamp.Controllers
{
    public class CategoryController : Controller
    {
        private JoukkoVideoDBContext db = new JoukkoVideoDBContext();

        //
        // GET: /Category/

        public ActionResult Index(int? id, int? page)
        {
            /**
            * Get videos ordered by timestamp (newest first).
            * The results are paginated.
            **/
          
            const int pageSize = 20;
            int pageNum = (page ?? 1);

            

            var category = db.Category.Find(id);

            var videos = from v in db.JoukkoVideos
                             from c in db.Category
                             where v.category.ID == c.ID
                             select v;

            videos = videos.OrderByDescending(v => v.timestamp);

            ViewBag.Category = category.name;
            var paginator = new PaginatedList<JoukkoVideo>(videos, pageNum, pageSize);
            
            return View(paginator);
            
        }

        // GET: /Category/list

        public ActionResult List(int? id, int? page)
        {
            /**
            * Get videos ordered by timestamp (newest first).
            * The results are paginated.
            **/

            var categories = db.Category.ToList();

            return View(categories);

        }

        //
        // GET: /Category/Create

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Category/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(string name)
        {
            Category category = new Category();
            category.name = name;
            db.Category.Add(category);
            db.SaveChanges();
            return View();
        }

    }
}
