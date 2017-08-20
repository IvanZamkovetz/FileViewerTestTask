using FileViewer.Model;
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
        public PathInfo PathInfo;

        public Viewer()
        {
        }
        public Viewer(PathInfo pathInfo)
        {
            PathInfo = pathInfo;
        }

        public bool Observe()
        {
            PathInfo.SubNodes.Clear();
            try
            {
                if (String.Equals(PathInfo.RootPath, "\\"))
                {
                    PathInfo.SmallFiles = -1;
                    PathInfo.MiddleFiles = -1;
                    PathInfo.BigFiles = -1;

                    PathInfo.SubNodes.AddRange(Directory.GetLogicalDrives());

                    return true;
                }
                if (Directory.Exists(PathInfo.RootPath))
                {
                    PathInfo.SubNodes.Add("..");

                    PathInfo.SubNodes.AddRange(Directory.GetDirectories(PathInfo.RootPath));
                    PathInfo.SubNodes.AddRange(Directory.GetFiles(PathInfo.RootPath));

                    return true;
                }

            }
            catch (UnauthorizedAccessException e)
            {
                return false;
            }
            catch (DirectoryNotFoundException e)
            {
                return false;
            }
            catch (IOException ex)
            {
                return false;
            }
            return false;
        }
        public string TryTransite(string newPath, string path)
        {
            try
            {
                if (Directory.GetDirectories(path).Contains(newPath))
                {
                    return newPath;
                }
                if (String.Equals(path, "\\") && Directory.GetLogicalDrives().Contains(newPath))
                {
                    return newPath;
                }
                if (String.Equals(newPath, ".."))
                {
                    DirectoryInfo parentPathInfo = Directory.GetParent(path);
                    return parentPathInfo == null ? "\\" : parentPathInfo.FullName;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public bool WidthBrowsePath()
        {
            if (String.Equals(PathInfo.RootPath, "\\") || !Directory.Exists(PathInfo.RootPath))
            {
                return false;
            }

            List<List<string>> pathsLevel = new List<List<string>>();
            pathsLevel.Add(new List<string>());
            pathsLevel.Add(new List<string>());

            int currentLevelIndex = 0;
            int nextLevelIndex = 1;

            int smallFiles = 0;
            int middleFiles = 0;
            int bigFiles = 0;

            pathsLevel[currentLevelIndex].Add(PathInfo.RootPath);
            do
            {
                pathsLevel[nextLevelIndex].Clear();
                foreach (string currentLevelPath in pathsLevel[currentLevelIndex])
                {
                    DirectoryInfo currentLevelPathInfo = new DirectoryInfo(currentLevelPath);
                    FileInfo[] filesInfo = null;
                    try
                    {
                        filesInfo = currentLevelPathInfo.GetFiles("*.*");
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
                        childPaths = Directory.GetDirectories(currentLevelPath);
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
                        continue;
                    }
                    pathsLevel[nextLevelIndex].AddRange(childPaths);
                }
                currentLevelIndex = nextLevelIndex == 1 ? 1 : 0;
                nextLevelIndex = currentLevelIndex == 1 ? 0 : 1;

            } while (pathsLevel[nextLevelIndex].Count != 0);

            PathInfo.SmallFiles = smallFiles;
            PathInfo.MiddleFiles = middleFiles;
            PathInfo.BigFiles = bigFiles;

            return true;
        }
        public bool DepthBrowsePath()
        {
            if (String.Equals(PathInfo.RootPath, "\\") || !Directory.Exists(PathInfo.RootPath))
            {
                return false;
            }

            int smallFiles = 0;
            int middleFiles = 0;
            int bigFiles = 0;

            //use Stack<Branch {string pathInfo, int childIndex}> paths for reducing next subNode search
            Stack<string> paths = new Stack<string>();
            paths.Push(PathInfo.RootPath);

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
                        popBuffer = paths.Pop();
                        pushed = false;
                        continue;
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        popBuffer = paths.Pop();
                        pushed = false;
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
                        popBuffer = paths.Pop();
                        pushed = false;
                        continue;
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        popBuffer = paths.Pop();
                        pushed = false;
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
                        popBuffer = paths.Pop();
                        pushed = false;
                        continue;
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        popBuffer = paths.Pop();
                        pushed = false;
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
                    else//if currentIndex was -1 than circumvention all subDirectories from beginning
                    {
                        paths.Push(childPaths[currentIndex + 1]);
                        pushed = true;
                        continue;
                    }
                }
            }

            PathInfo.SmallFiles = smallFiles;
            PathInfo.MiddleFiles = middleFiles;
            PathInfo.BigFiles = bigFiles;

            return true;
        }

        public static void CleanDataBase()
        {
            FileViewerContext context = new FileViewerContext();
            List<PathInfo> pathInfos = context.PathInfos.ToList();
            foreach (PathInfo pathInfo in pathInfos)
            {
                if (!Directory.Exists(pathInfo.RootPath))
                {
                    context.Entry(pathInfo).State = System.Data.Entity.EntityState.Deleted;
                }
            }

            context.SaveChanges();

            return;
        }
    }
}
