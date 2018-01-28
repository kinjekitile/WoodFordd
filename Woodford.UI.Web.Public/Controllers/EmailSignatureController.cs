using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainServices;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;


namespace Woodford.UI.Web.Public.Controllers {
    public class EmailSignatureController : Controller {

        private IQueryProcessor _query;
        private ICommandBus _commandBus;

        public EmailSignatureController(IQueryProcessor query, ICommandBus commandBus) {
            _query = query;
            _commandBus = commandBus;
        }

        // GET: EmailSignature
        public ActionResult Index(int id) {
            var fileData = _query.Process(new EmailSignatureGetImageQuery { Id = id });
            string mimeType = MimeTypes.Get("png");
            return File(fileData, mimeType);

        }
    }
}