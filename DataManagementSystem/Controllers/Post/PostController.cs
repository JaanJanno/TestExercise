using DataManagementSystem.Controllers.Post.Auth;
using DataManagementSystem.Controllers.Post.Decrypt;
using DataManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace DataManagementSystem.Controllers
{
    public class PostController : Controller
    {   

        // Retrieves posts of encrypted JSON data.
        [HttpPost]
        public HttpStatusCodeResult Index()
        {
            // Does HTTP Basic authentication.
            if (Auth.authenticate(Request))
                // If processing the request is successful, return OK.
                if(processPost(Request))
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                else
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        // Tries to save received/decrypted JSON to database.
        // Returns true, if successful.
        private static Boolean processPost(HttpRequestBase request)
        {
            try
            {
                // Decrypts the JSON received from the request.
                string encryptedJSON = readRequestBody(request);
                string jsonString = Decrypt.DecryptString(encryptedJSON);
                // Saves JSON to database.
                JSONEntriesController.Create(jsonString);
                return true;
            }
            catch
            {
                return false;
            }        
        }

        // Extracts the body of a request in string format.
        private static string readRequestBody(HttpRequestBase request)
        {
            string body;
            using (var reader = new StreamReader(request.InputStream))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}