using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;
namespace Woodford.UI.Web.Admin.Controllers {


    [Authorize(Roles = "Administrator,SEO")]
    public class AuditController : Controller {

        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public AuditController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult RenderAuditContainer(string entityType, int entityIdValue, bool pageContent = false, int pageContentId = 0) {
            AuditViewModel viewModel = new AuditViewModel();
            viewModel.EntityType = entityType;
            viewModel.EntityKeyValue = entityIdValue;
            viewModel.HasPageContent = pageContent;
            viewModel.PageContentId = pageContentId;
            return PartialView("_AuditLogContainer", viewModel);
        }


        public ActionResult AuditLog(string entityType, int entityId, int p = 1) {

            //entityType = entityType.Replace("Woodford.Core.DomainModel.Models.", "");

            string dbEntityType = Utilities.GetAuditEntityNameForModel(entityType);

            AuditGetQuery query = new AuditGetQuery { Filter = new AuditItemFilterModel { EntityType = dbEntityType, EntityId = entityId }, Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 10 } };

            var results = _query.Process(query);

            return PartialView("_AuditLog", results);
        }
    }
}