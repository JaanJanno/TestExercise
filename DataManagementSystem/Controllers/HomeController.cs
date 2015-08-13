using DataManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataManagementSystem.Controllers
{
    public class HomeController : Controller
    {

        // Main view - shows list of all received JSON strings.
        public ActionResult Index()
        {
            return View(JSONEntriesController.Get());
        }
    }
}