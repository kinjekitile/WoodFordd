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
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers
{
    public class BranchesController : Controller
    {
        private IQueryProcessor _query;
        private ICommandBus _commandBus;

        public BranchesController(IQueryProcessor query, ICommandBus commandBus) {
            _query = query;
            _commandBus = commandBus;   
        }

        [Route("branches")]
        public ActionResult Index()
        {
            BranchesGetQuery query = new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false } };
            var branches = _query.Process(query);

            return View(branches);
        }

        [Route("branches/{branchUrl?}")]
        public ActionResult Branch(string branchUrl) {

            BranchGetByUrlQuery query = new BranchGetByUrlQuery { Url = branchUrl, IncludePageContent = true };
            BranchModel b = _query.Process(query);
            return View(b);
        }

        [Route("branches/contact/{branchUrl?}")]
        public ActionResult Contact(string branchUrl) {

            BranchGetByUrlQuery query = new BranchGetByUrlQuery { Url = branchUrl, IncludePageContent = true };
            BranchModel b = _query.Process(query);

            BranchContactViewModel model = new BranchContactViewModel { Branch = b, ContactUs = new ContactUsNotificationModel() };

            return View(model);
        }

        [Route("branches/contact/{branchUrl?}")]
        [HttpPost]
        public ActionResult Contact(string branchUrl, [CustomizeValidator(RuleSet = BranchContactValidationRuleSets.Default)] BranchContactViewModel model) {

            BranchGetByUrlQuery query = new BranchGetByUrlQuery { Url = branchUrl, IncludePageContent = true };
            BranchModel b = _query.Process(query);
            model.Branch = b;
            model.ContactUs.Branch = b.Title;

            if (ModelState.IsValid) {
                NotifyContactUsCommand command = new NotifyContactUsCommand { Contact = model.ContactUs };
                _commandBus.Submit(command);
                return View("ContactSent", model);
            }
            
            return View(model);
        }

        [Route("branches/requestcallback/{branchUrl?}")]
        public ActionResult RequestCallback(string branchUrl) {

            BranchGetByUrlQuery query = new BranchGetByUrlQuery { Url = branchUrl, IncludePageContent = true };
            BranchModel b = _query.Process(query);

            BranchRequestCallbackViewModel model = new BranchRequestCallbackViewModel {  Branch = b, RequestCallback = new RequestCallbackNotificationModel() };

            return View(model);
        }

        [Route("branches/requestcallback/{branchUrl?}")]
        [HttpPost]
        public ActionResult RequestCallback(string branchUrl, [CustomizeValidator(RuleSet = BranchRequestCallbackValidationRuleSets.Default)] BranchRequestCallbackViewModel model) {

            BranchGetByUrlQuery query = new BranchGetByUrlQuery { Url = branchUrl, IncludePageContent = true };
            BranchModel b = _query.Process(query);
            model.Branch = b;
            model.RequestCallback.Branch = b.Title;

            if (ModelState.IsValid) {
                NotifyRequestCallbackCommand command = new NotifyRequestCallbackCommand { RequestCallback = model.RequestCallback };
                _commandBus.Submit(command);
                return View("RequestCallbackSent", model);
            }

            return View(model);
        }
    }
}