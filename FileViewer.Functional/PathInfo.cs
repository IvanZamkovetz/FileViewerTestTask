using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileViewer.Functional
{
   public class PathInfo
    {
        public string RootPath;

        public List<string> SubNodes;

        public int SmallFiles;
        public int MiddleFiles;
        public int BigFiles;

        public PathInfo()
        {
            SubNodes = new List<string>();
        }
        public PathInfo ShallowCopy()
        {
            return (PathInfo)this.MemberwiseClone();
        }
    }
}
