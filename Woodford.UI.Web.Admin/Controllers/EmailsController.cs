using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "SystemAdministrator")]
    public class EmailsController : Controller
    {
        private ICommandBus _commandBus;
        private IQueryProcessor _query;

        public EmailsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;

        }

        // GET: Messages
        public ActionResult Index(int p = 1)
        {

            MessageQueueGetQuery query = new MessageQueueGetQuery { Filter = null, Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 } };
            ListOf<MessageQueueModel> emails = _query.Process(query);
            return View(emails);
        }
    }
}