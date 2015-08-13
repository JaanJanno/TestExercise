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
        // Main view with zip upload form.
        public ActionResult Index()
        {
            return View();
        }

        // Post of the zip file upload form.
        [HttpPost]
        public ActionResult Post()
        {
            string username, password, json;
            HttpPostedFileBase file;

            try // Extracting the form contents from request.
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

            try // Uploaded zip file structure interpreted to JSON.
            {
                json = ZipParser.zipToJSONString(file);
            }
            catch
            {
                TempData["Message"] = "Invalid zip file";
                return new RedirectResult("/");
            }

            // Makes request to DataManagementSystem
            switch (RequestMaker.makeReqest(json, username, password)){
                    // User gets a different message depending 
                    // on the response from DataManagementSystem.
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