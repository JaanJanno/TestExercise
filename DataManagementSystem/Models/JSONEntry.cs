using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataManagementSystem.Models
{
    public class JSONEntry
    {
        public int ID { get; set; }
        public string content { get; set; }

        public override string ToString() {
            return content;
        }
    }

    public class JSONEntryContext : DbContext
    {
        public DbSet<JSONEntry> Entries { get; set; }
    }
}