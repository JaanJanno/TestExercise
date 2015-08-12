using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataManagementSystem.Models;

namespace DataManagementSystem.Controllers
{
    public class JSONEntriesController
    {
        private static JSONEntryContext db = new JSONEntryContext();

        public static void Create(string jsonString)
        {
            JSONEntry jsonEntry = new JSONEntry();
            jsonEntry.content = jsonString;
            db.Entries.Add(jsonEntry);
            db.SaveChangesAsync();
        }

        public static List<JSONEntry> Get()
        {
            return db.Entries.ToList();
        }
    }
}
