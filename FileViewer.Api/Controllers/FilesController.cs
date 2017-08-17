using FileViewer.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;

namespace FileViewer.Api.Controllers
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiController
    {
        public FilesController()
        {
        }

        [HttpGet]
        public PathInfo GetDefaultPathInfo()
        {
            Viewer viewer = new Viewer();
            PathInfo pathInfo;

            viewer.BrowsePath();
            pathInfo = viewer.PathInfo;


            return pathInfo;
        }
        [HttpGet]
        public PathInfo GetPathInfo(string newPath, string basePath, int smallFiles, int middleFiles, int bigFiles)
        {
            Viewer viewer = new Viewer();
            PathInfo pathInfo;

            viewer.RootPath = newPath;
            viewer.BrowsePath();
            pathInfo = viewer.PathInfo;

            return pathInfo;
        }

        public PathInfo UpdatePathInfo(string basePath)
        {
            PathInfo pathInfo = new PathInfo();

            return pathInfo;
        }
        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
