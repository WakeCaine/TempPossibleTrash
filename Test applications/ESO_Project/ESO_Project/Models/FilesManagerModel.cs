using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESO_Project.Models
{
    public class FilesManagerModel
    {

        [FileSize(1000024000, ErrorMessage = "Maximum allowed file size is {0} bytes")]
        public HttpPostedFileBase File { get; set; }
        public List<ESO_Project.Entities.File> files { get; set; }
    }
}