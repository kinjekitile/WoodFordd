using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers
{
    public class AdvanceController : Controller
    {
        private ICommandBus _commandBus;
        private IQueryProcessor _query;

        public AdvanceController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        [AllowAnonymous]
        public ActionResult Index() {
            return RedirectToAction("Enroll");
        }

        // GET: Advance
        [AllowAnonymous]
        public ActionResult Enroll() {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Enroll([CustomizeValidator(RuleSet = UserRegistrationValidationRuleSets.Default)] UserRegistrationViewModel model) {
            if (ModelState.IsValid) {

                UserRegisterCommand command = new UserRegisterCommand { User = model.User, Password = model.UserPassword };
                _commandBus.Submit(command);
                if (command.Success) {
                    return RedirectToAction("Advance", "User");
                }
            }
            return View(model);
        }

        public ActionResult Faqs() {
            return View();
        }

    }
}