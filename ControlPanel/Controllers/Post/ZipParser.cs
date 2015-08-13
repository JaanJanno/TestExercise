using ControlPanel.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;

namespace ControlPanel.Controllers.Post
{

    /*
     * Class for interpreting a zip file 
     * folder structure to JSON format.
     */

    public class ZipParser
    {

        // Returns zip file folder structure in JSON format.
        public static string zipToJSONString(HttpPostedFileBase file)
        {
            ZipArchive zip = new ZipArchive(file.InputStream);
            ZipTree tree = parseFolder(zip.Entries.ToList(), file.FileName, "", 0);
            return tree.ToString();
        }

        // Given a list of entries in a zip file, transforms their structure to JSON.
        private static Folder parseFolder(List<ZipArchiveEntry> zip, string filename, string name, int level)
        {
            Folder f = (filename == null ? new Folder(folderName(name)) : new Folder(filename));

            List<ZipArchiveEntry> matchedFiles = zip.FindAll(x => x.FullName.StartsWith(name) && (fileLevel(x.FullName) == level) && !isFolder(x.FullName));
            List<ZipArchiveEntry> matchedFolders = zip.FindAll(x => x.FullName.StartsWith(name) && (fileLevel(x.FullName) == level + 1) && isFolder(x.FullName));

            foreach(ZipArchiveEntry e in matchedFiles) 
            {
                f.fileEntries.Add(new File(e.Name));
            }          
            foreach (ZipArchiveEntry e in matchedFolders)
            {
                f.folderEntries.Add(parseFolder(zip, null, e.FullName, level+1));
            }

            return f;
        }

        // Determines the depth of a file in the folder structure tree.
        private static int fileLevel(string name)
        {
            // Count of '/' characters in a file's URL determines its
            // depth in the folder tree.
            int slashCount = name.Count(c => c == '/');
            return slashCount;
        }

        // Returns the name of a folder given its URL.
        private static string folderName(string name)
        {
            name = name.Substring(0, name.Length - 1);
            if (name.Contains("/"))
            {
                return name.Substring(name.LastIndexOf("/") + 1);
            }
            else
                return name;
        }

        // Determines if an entity in a zip file is a folder.
        private static Boolean isFolder(string str)
        {
            // If an entities name URL with '/' it is a folder.
            if (str.EndsWith("/"))
                return true;
            else
                return false;
        }
    }
}