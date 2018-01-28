using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "SystemAdministrator")]
    public class SettingsController : Controller
    {
        private ICommandBus _commandBus;
        private IQueryProcessor _query;

        public SettingsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;

        }
                
        public ActionResult Index()
        {
            SettingGetAllQuery query = new SettingGetAllQuery();
            List<SettingModel> settings = _query.Process(query);

            return View(settings);
        }

        public ActionResult Edit(string type) {
            Setting s = (Setting)Enum.Parse(typeof(Setting), type);
            SettingGetByNameQuery query = new SettingGetByNameQuery { SettingName = s };
            SettingModel setting = _query.Process(query);
            return View(setting);
        }

        [HttpPost] 
        public ActionResult Edit(string type, SettingModel model) {
            Setting s = (Setting)Enum.Parse(typeof(Setting), type);
            model.Type = s;
            if (ModelState.IsValid) {
                SettingEditCommand command = new SettingEditCommand { SettingName = s, SettingValue = model.Value };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);


        }

    }
}