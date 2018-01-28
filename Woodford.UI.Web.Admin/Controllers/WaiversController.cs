using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class WaiversController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public WaiversController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        public ActionResult Index()
        {
            WaiversViewModel model = new WaiversViewModel();
            model.VehicleGroups = getVehicleGroups();
            model.Waivers = getWaivers();
            return View(model);
        }

        #region api methods        
        public ActionResult GetWaiverTypes() {

            List<WaiverTypeAPIModel> types = new List<WaiverTypeAPIModel>();

            foreach (var waiverType in Enum.GetValues(typeof(WaiverType))) {
                types.Add(new WaiverTypeAPIModel { Id = Convert.ToInt32(waiverType), Title = waiverType.ToString() });                
            }
            return Json(types, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddWaiver(string waiver, string cost, string liability, string vehicleGroupId) {

            int wtId = Convert.ToInt32(waiver);
            int vgId = Convert.ToInt32(vehicleGroupId);
            decimal? wCost = null;
            decimal? wLiability = null;
            if (!string.IsNullOrEmpty(cost)) {
                wCost = Convert.ToDecimal(cost);
            }
            if (!string.IsNullOrEmpty(liability)) {
                wLiability = Convert.ToDecimal(liability);
            }

            WaiverModel w = new WaiverModel();
            w.VehicleGroupId = vgId;
            w.WaiverType = (WaiverType)wtId;
            w.CostPerDay = wCost;
            w.Liability = wLiability;

            WaiverAddCommand command = new WaiverAddCommand { Model = w };
            _commandBus.Submit(command);

            return Json(command.Model.Id, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult EditWaiver(string waiverId, string waiver, string cost, string liability) {

            int wId = Convert.ToInt32(waiverId);
            int wtId = Convert.ToInt32(waiver);
            decimal? wCost = null;
            decimal? wLiability = null;
            if (!string.IsNullOrEmpty(cost)) {
                wCost = Convert.ToDecimal(cost);
            }
            if (!string.IsNullOrEmpty(liability)) {
                wLiability = Convert.ToDecimal(liability);
            }

            WaiverModel w = new WaiverModel();
            w.Id = wId;
            w.WaiverType = (WaiverType)wtId;
            w.CostPerDay = wCost;
            w.Liability = wLiability;

            WaiverEditCommand command = new WaiverEditCommand { Model = w };
            _commandBus.Submit(command);

            return Json(command.Model.Id, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult GetWaiver(int waiverId) {
            return Json(getWaiver(waiverId), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult DeleteWaiver(int waiverId) {
            WaiverDeleteCommand command = new WaiverDeleteCommand { Id = waiverId };
            _commandBus.Submit(command);
            return Json("success", JsonRequestBehavior.DenyGet);
        }

        #endregion


        #region helper methods

        private List<VehicleGroupModel> getVehicleGroups() {
            VehicleGroupsGetQuery query = new VehicleGroupsGetQuery();
            ListOf<VehicleGroupModel> vg = _query.Process(query);
            if (vg == null) {
                return null;
            } else {
                return vg.Items;
            }            
        }

        public List<WaiverModel> getWaivers() {
            WaiversGetQuery query = new WaiversGetQuery();
            return _query.Process(query);
        }

        public WaiverModel getWaiver(int id) {
            WaiverGetByIdQuery query = new WaiverGetByIdQuery { Id = id };
            return _query.Process(query);
        }

        #endregion
    }
}