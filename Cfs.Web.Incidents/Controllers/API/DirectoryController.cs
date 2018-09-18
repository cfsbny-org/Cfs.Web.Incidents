using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class DirectoryController : ApiController
    {
        //[HttpGet, Route("api/directory}")]
        public List<Models.Presentation.DirectoryViewModel> Get(string path)
        {
            var directories = (from subdirectory in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly)
                   //where Directory.GetDirectories(subdirectory).Length == 0
                   select subdirectory).ToList();


            List<Models.Presentation.DirectoryViewModel> directoryList = new List<Models.Presentation.DirectoryViewModel>();

            foreach (string directory in directories)
            {
                directoryList.Add(new Models.Presentation.DirectoryViewModel()
                {
                    fullPath = directory,
                    folderName = directory.Substring(directory.LastIndexOf('\\') + 1)
                });
            }

            return directoryList;
        }



        public void Post([FromBody]string folderPathName)
        {
            System.IO.Directory.CreateDirectory(folderPathName);
        }


    }
}