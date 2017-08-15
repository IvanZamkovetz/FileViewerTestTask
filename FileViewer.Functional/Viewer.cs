using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileViewer.Functional
{
    public class Viewer
    {
        private string _rootPath;

        private List<string> _subNodes;

        private int _smallFiles;
        public int _middleFiles;
        public int _bigFiles;

        public string RootPath
        {
            get { return _rootPath; }
            set
            {
                if (value != "")
                {
                    //check if it's directory

                    if (value == "..")
                    {
                        _rootPath = Directory.GetParent(_rootPath).FullName;
                    }
                    else
                    {
                        _rootPath = value;
                    }
                    _subNodes.AddRange(Directory.GetDirectories(_rootPath));
                    _subNodes.AddRange(Directory.GetFiles(_rootPath));
                }

            }
        }
        public List<string> SubNodes { get { return _subNodes; } }
        public int SmallFiles { get { return _smallFiles; } }
        public int MiddleFiles { get { return _middleFiles; } }
        public int BigFiles { get { return _bigFiles; } }

        public Viewer()
        {
            _rootPath = Directory.GetCurrentDirectory();
        }

        public void BrowsePath()
        {
            _smallFiles = 0;
            _middleFiles = 0;
            _bigFiles = 0;

            Stack<string> paths = new Stack<string>();
            paths.Push(_rootPath);

            while (paths.Count > 0)
            {
                DirectoryInfo currentPath = new DirectoryInfo(paths.Peek());
                FileInfo[] files = null;
                try
                {
                    files = currentPath.GetFiles("*.*");
                }
                catch (UnauthorizedAccessException e)
                {
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    continue;
                }
                foreach (FileInfo file in files)
                {
                    if (file.Length <= 10000)
                    {
                        _smallFiles++;
                    }
                    if (file.Length > 10000 && file.Length <= 50000)
                    {
                        _middleFiles++;
                    }
                    if (file.Length >= 100000)
                    {
                        _bigFiles++;
                    }
                }

                string[] childPaths;
                try
                {
                    childPaths = Directory.GetDirectories(paths.Peek());
                }
                catch (UnauthorizedAccessException e)
                {
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    continue;
                }
                if (childPaths.Count() > 0)
                {
                    paths.Push(childPaths[0]);
                    continue;
                }
                string finalizedPath = paths.Pop();
                childPaths = Directory.GetDirectories(paths.Peek());

            }
        }
    }
}
