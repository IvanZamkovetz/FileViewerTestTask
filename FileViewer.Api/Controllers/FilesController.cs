using FileViewer.Api.Models;
using FileViewer.Api.Services;
using FileViewer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using System.Web.Http.Cors;

namespace FileViewer.Api.Controllers
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiController
    {
        public static bool DataBaseCleaned = false;

        public FileViewerContext Context { get; set; }

        public FilesController()
        {
            Context = new FileViewerContext();

            if (!DataBaseCleaned)
            {
                Viewer viewer = new Viewer();
                Task.Run(() => Viewer.CleanDataBase());

                DataBaseCleaned = true;
            }
        }

        [HttpGet]
        public PathInfo GetDefaultPath()
        {
            string defaultPath = Directory.GetCurrentDirectory();
            PathInfo pathInfo = Context.PathInfos.SingleOrDefault(p => String.Equals(p.RootPath, defaultPath));

            if (pathInfo == null)
            {
                pathInfo = new PathInfo();
                pathInfo.RootPath = defaultPath;
            }

            Viewer viewer = new Viewer(pathInfo);
            viewer.Observe();

            return pathInfo;
        }
        [Route("transite")]
        [HttpPost]
        public PathInfo TransiteToPath(RootPath rootPath)
        {
            Viewer viewer = new Viewer();
            string transitedPath = viewer.TryTransite(rootPath.NewPath, rootPath.Path);
            if (transitedPath == null)
            {
                return null;
            }

            PathInfo pathInfo = Context.PathInfos.SingleOrDefault(p => String.Equals(p.RootPath, transitedPath));

            if (pathInfo == null)
            {
                pathInfo = new PathInfo();
                pathInfo.RootPath = transitedPath;
            }

            viewer.PathInfo = pathInfo;
            if (!viewer.Observe())
            {
                return null;
            }

            return pathInfo;
        }
        [Route("count")]
        [HttpPost]
        public PathInfo CountPathFiles(RootPath rootPath)
        {
            PathInfo pathInfo = Context.PathInfos.Where(pi => String.Equals(pi.RootPath, rootPath.Path)).SingleOrDefault();
            if (pathInfo == null)
            {
                pathInfo = new PathInfo();
                pathInfo.RootPath = rootPath.Path;
                Context.Entry(pathInfo).State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                Context.Entry(pathInfo).State = System.Data.Entity.EntityState.Modified;
            }
            Viewer viewer = new Viewer(pathInfo);
            if (!viewer.WidthBrowsePath())
            {
                Context.Entry(pathInfo).State = System.Data.Entity.EntityState.Detached;
                return null;
            }
            Context.SaveChanges();

            return pathInfo;
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }

        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
            base.Dispose(disposing);
        }
    }
}
