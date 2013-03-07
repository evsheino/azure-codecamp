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
            ViewBag.Category = category.name;
            var videos = category.videos.AsQueryable();
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

    }
}
