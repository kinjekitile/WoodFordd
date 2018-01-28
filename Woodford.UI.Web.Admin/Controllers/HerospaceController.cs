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
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator,SEO")]
    public class HerospaceController : Controller
    {
        
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public HerospaceController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        public ActionResult Index()
        {
            HerospaceItemsGetQuery query = new HerospaceItemsGetQuery();            
            List<HerospaceItemModel> items = _query.Process(query);
            return View(items);
        }

        public ActionResult Add() {
            return View(new HerospaceItemViewModel());
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = HerospaceValidationRuleSets.Default)] HerospaceItemViewModel model) {
            if (ModelState.IsValid) {
                model.Item.HerospaceImage = FileUploadModelFactory.CreateInstance(model);

                HerospaceAddCommand command = new HerospaceAddCommand { Model = model.Item };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {
            HerospaceItemGetByIdQuery query = new HerospaceItemGetByIdQuery { Id = id };
            HerospaceItemViewModel i = new HerospaceItemViewModel { Item = _query.Process(query) };

            return View(i);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, [CustomizeValidator(RuleSet = HerospaceValidationRuleSets.Default)] HerospaceItemViewModel model) {
            if (ModelState.IsValid) {

                model.Item.HerospaceImage = FileUploadModelFactory.CreateInstance(model);

                HerospaceEditCommand command = new HerospaceEditCommand { Model = model.Item };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            } else {
                HerospaceItemGetByIdQuery query = new HerospaceItemGetByIdQuery { Id = id };
                HerospaceItemModel i = _query.Process(query);
                model.Item.HerospaceImage = i.HerospaceImage;
            }
            return View(model);
        }
    }
}