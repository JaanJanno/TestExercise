using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlPanel.Models
{

    /*
     * Class for describing folder structure trees.
     */

    public abstract class ZipTree
    {
        public string Name { get; set; }
    }

    public class File : ZipTree
    {
        public File(string name)
        {
            this.Name = name;
        }

        // Returns its info in JSON format.
        public override string ToString()
        {
            return "{\"name\":\"" + this.Name + "\"}";
        }
    }

    public class Folder : ZipTree
    {
        public List<File> fileEntries { get; set; }
        public List<Folder> folderEntries { get; set; }

        public Folder(string name)
        {
            this.Name = name;
            this.fileEntries = new List<File>();
            this.folderEntries = new List<Folder>();
        }

        // Returns its info in JSON format.
        public override string ToString()
        {
            string ret = "{\"name\":\"" + this.Name + "\",\"files\":[";
            foreach (ZipTree z in fileEntries){
                ret += z.ToString()+",";
            }
            if (fileEntries.Count != 0)
                ret = ret.Substring(0, ret.Length-1);
            ret += "],\"folders\":[";
            foreach (ZipTree z in folderEntries)
            {
                ret += z.ToString()+",";
            }
            if (folderEntries.Count != 0)
                ret = ret.Substring(0, ret.Length-1);
            return ret + "]}";
        }
    }
}