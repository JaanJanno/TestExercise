using ControlPanel.Controllers.Post;
using ControlPanel.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlPanel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post()
        {
            string username, password, json;
            HttpPostedFileBase file;

            try
            {
                username = Request.Form["username"];
                password = Request.Form["password"];
                file = Request.Files["file"];
            }
            catch
            {
                TempData["Message"] = "Invalid data in form";
                return new RedirectResult("/");
            }

            try
            {
                json = ZipParser.zipToJSONString(file);
            }
            catch
            {
                TempData["Message"] = "Invalid zip file";
                return new RedirectResult("/");
            }

            switch (RequestMaker.makeReqest(json, username, password)){
                case 200:
                    {
                        TempData["Message"] = "Successfully sent zip";
                        return new RedirectResult("/");
                    }
                case 401:
                    {
                        TempData["Message"] = "Invalid username or password";
                        return new RedirectResult("/");
                    }
                default:
                    {
                        TempData["Message"] = "Problem sending data";
                        return new RedirectResult("/");
                    }
            }
        }

        
    }
}