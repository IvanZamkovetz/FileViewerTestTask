using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileViewer.Model
{
    public class PathInfo
    {
        public int Id { get; set; }
        public string RootPath { get; set; }

        public int SmallFiles { get; set; }
        public int MiddleFiles { get; set; }
        public int BigFiles { get; set; }

        [NotMapped]
        public List<string> SubNodes;

        public PathInfo()
        {
            SubNodes = new List<string>();

            SmallFiles = -1;
            MiddleFiles = -1;
            BigFiles = -1;
        }

    }
}
