using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers {
    public class ContactController : Controller {
        private ICommandBus _commandBus;

        public ContactController(ICommandBus commandBus) {
            _commandBus = commandBus;
        }
        public ActionResult Index() {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public ActionResult Index([CustomizeValidator(RuleSet = ContactValidationRuleSets.Default)] ContactViewModel model) {
            if (ModelState.IsValid) {
                NotifyContactUsCommand command = new NotifyContactUsCommand { Contact = model.ContactUs };

                _commandBus.Submit(command);
                return View("Sent", model);
            }
            return View(model);
        }

        public ActionResult RequestCallback() {
            return View(new RequestCallbackViewModel());
        }

        [HttpPost]
        public ActionResult RequestCallback([CustomizeValidator(RuleSet = RequestCallbackValidationRuleSets.Default)] RequestCallbackViewModel model) {
            if (ModelState.IsValid) {
                NotifyRequestCallbackCommand command = new NotifyRequestCallbackCommand { RequestCallback = model.RequestCallback };
                _commandBus.Submit(command);
                return View("RequestCallbackSent", model);
            }
            return View(model);
        }
    }
}