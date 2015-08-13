using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ControlPanel.Controllers.Post
{

    /*
     * Class for making an HTTP request to DataManagementSystem.
     */

    public class RequestMaker
    {

        // Sends a JSON string using authentication.
        public static int makeReqest(string json, string username, string password)
        {
            WebRequest request = WebRequest.Create("http://localhost:57827/post");
            request.Method = "POST";
            addAuthHeader(request, username, password);

            string encryptedJson = Encrypt.EncryptString(json);
            byte[] byteArray = Encoding.UTF8.GetBytes(encryptedJson);
            request.ContentType = "text/plain";
            request.ContentLength = byteArray.Length;

            if (handleRequest(request, byteArray))
                return handleResponse(request);
            else
                return 0;
        }

        // Sends request to DataManagementSystem
        private static Boolean handleRequest(WebRequest request, byte[] byteArray)
        {
            try
            {
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Returns the status code received as response from DataManagementSystem.
        // Returns 0, if no status code can be extracted.
        private static int handleResponse(WebRequest request)
        {
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return ((int)response.StatusCode);
                }
            }
            catch (WebException except)
            {
                if (except.Status == WebExceptionStatus.ProtocolError)
                {
                    var exceptResponse = except.Response as HttpWebResponse;
                    if (exceptResponse != null)
                    {
                        return ((int)exceptResponse.StatusCode);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        // Adds HTTP basic authentication header to request.
        private static void addAuthHeader(WebRequest request, string username, string password)
        {
            string credentials = username + ":" + password;
            credentials = Convert.ToBase64String(Encoding.Default.GetBytes(credentials));
            request.Headers["Authorization"] = "Basic " + credentials;
        }
    }
}