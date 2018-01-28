using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Controllers
{
    public class UploadsController : Controller
    {

        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public UploadsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        // GET: Uploads
        //[OutputCache]
        public FileResult Index(int id, string fileName, int? width, int? height, int? crop)
        {
            FileGetByIdQuery query = new FileGetByIdQuery { Id = id, Width = width, Height = height, Crop = crop };
            FileUploadModel f = _query.Process(query);
            if (f == null) {
                return null;
            }
            return File(f.FileContents, f.MimeType);
        }
    }
}