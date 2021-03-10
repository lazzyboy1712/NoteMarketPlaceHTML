using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using notesmarketplace.Models;
using System.Net;

namespace notesmarketplace.Controllers
{
    public class HomeController : Controller
    {
        NotesMarketPlaceEntities db = new NotesMarketPlaceEntities();
        public ActionResult Index()
        {
            return View();
        }
    }
}