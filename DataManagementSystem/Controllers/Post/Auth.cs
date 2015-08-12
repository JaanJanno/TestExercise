using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace DataManagementSystem.Controllers.Post.Auth
{
    
    // Does HTTP Basic authentication.

    public class Auth
    {
        private static Encoding encoding = Encoding.GetEncoding("iso-8859-1");

        //Checks validity of request's HTTP basic authentication.

        public static Boolean authenticate(HttpRequestBase request)
        {
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                var credentials = AuthenticationHeaderValue.Parse(authHeader);
                var credentialsString = encoding.GetString(Convert.FromBase64String(credentials.Parameter));
                int separator = credentialsString.IndexOf(':');
                string username = credentialsString.Substring(0, separator);
                string password = credentialsString.Substring(separator + 1);
                return validateCredentials(username, password);
            }
            else
                return false;
        }

        //Compares the username and password from Web.config file to its arguments.

        private static Boolean validateCredentials(string username, string password)
        {
            // Retrieves the username and password info from Web.config
            string correctUsername = WebConfigurationManager.AppSettings["username"];
            string correctPassword = WebConfigurationManager.AppSettings["password"];

            if (password.Equals(correctPassword) && username.Equals(correctUsername))
                return true;
            else
                return false;
        }
    }
}