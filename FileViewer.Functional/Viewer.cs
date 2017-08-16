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
        private PathInfo _pathInfo;

        public PathInfo PathInfo { get { return _pathInfo.ShallowCopy(); } }
        public string RootPath
        {
            get { return _pathInfo.RootPath; }
            set
            {
                if (value != "")
                {
                    //check if it's directory

                    if (value == "..")
                    {
                        _pathInfo.RootPath = Directory.GetParent(_pathInfo.RootPath).FullName;
                    }
                    else
                    {
                        _pathInfo.RootPath = value;
                    }

                    _pathInfo.SubNodes.Clear();

                    _pathInfo.SubNodes.Add("..");
                    try
                    {
                        _pathInfo.SubNodes.AddRange(Directory.GetDirectories(_pathInfo.RootPath));
                        _pathInfo.SubNodes.AddRange(Directory.GetFiles(_pathInfo.RootPath));
                    }
                    catch (UnauthorizedAccessException e)
                    {

                    }
                    catch (DirectoryNotFoundException e)
                    {

                    }
                }

            }
        }

        public Viewer()
        {
            _pathInfo = new PathInfo();
            _pathInfo.RootPath = Directory.GetCurrentDirectory();
            _pathInfo.SubNodes.Add("..");
            try
            {
                _pathInfo.SubNodes.AddRange(Directory.GetDirectories(_pathInfo.RootPath));
                _pathInfo.SubNodes.AddRange(Directory.GetFiles(_pathInfo.RootPath));
            }
            catch (UnauthorizedAccessException e)
            {

            }
            catch (DirectoryNotFoundException e)
            {

            }
        }

        public void BrowsePath()
        {
            int smallFiles = 0;
            int middleFiles = 0;
            int bigFiles = 0;

            Stack<string> paths = new Stack<string>();
            paths.Push(_pathInfo.RootPath);

            bool pushed = true;
            string popBuffer = "";

            while (paths.Count > 0)
            {
                if (pushed)
                {
                    DirectoryInfo currentPathInfo = new DirectoryInfo(paths.Peek());
                    FileInfo[] filesInfo = null;
                    try
                    {
                        filesInfo = currentPathInfo.GetFiles("*.*");
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        continue;
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        continue;
                    }
                    foreach (FileInfo fileInfo in filesInfo)
                    {
                        if (fileInfo.Length <= 10000)
                        {
                            smallFiles++;
                        }
                        if (fileInfo.Length > 10000 && fileInfo.Length <= 50000)
                        {
                            middleFiles++;
                        }
                        if (fileInfo.Length >= 100000)
                        {
                            bigFiles++;
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
                    if (childPaths.Length == 0)
                    {
                        popBuffer = paths.Pop();
                        pushed = false;
                        continue;
                    }
                    else
                    {
                        paths.Push(childPaths[0]);
                        pushed = true;
                        continue;
                    }
                }
                else
                {
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
                    int currentIndex = Array.IndexOf(childPaths, popBuffer);
                    if (currentIndex == -1)
                    { }
                    if (currentIndex == childPaths.Length - 1)
                    {
                        popBuffer = paths.Pop();
                        pushed = false;
                        continue;
                    }
                    else
                    {
                        paths.Push(childPaths[currentIndex + 1]);
                        pushed = true;
                        continue;
                    }
                }
            }

            _pathInfo.SmallFiles = smallFiles;
            _pathInfo.MiddleFiles = middleFiles;
            _pathInfo.BigFiles = bigFiles;
        }
    }
}
