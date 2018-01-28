using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator")]
    public class RatesController : Controller {

        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public RatesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        // GET: Rates
        public ActionResult Index() {
            BranchesGetQuery query = new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false } };
            ListOf<BranchModel> branches = _query.Process(query);

            return View(new RateSearchViewModel { Branches = branches.Items, SelectedBranches = new List<BranchModel>() });
        }

        [HttpPost]
        public ActionResult Index(RateSearchViewModel model) {

            //BranchesGetQuery query = new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false } };
            //ListOf<BranchModel> branches = _query.Process(query);

            //model.Branches = branches.Items;
            //model.SelectedBranches = model.Branches.Where(x => model.SelectedBranchIds.Contains(x.Id)).ToList();

            //if (ModelState.IsValid) {
            //    model.Filter.BranchIds = model.SelectedBranchIds.ToList();
            //    RateSearchQuery querySearch = new RateSearchQuery { Filter = model.Filter };
            //    model.Results = _query.Process(querySearch);
            //}
            return View(model);
        }

        public ActionResult Add(string branches, int ratecodeid) {

            ViewBag.Branches = branches;
            ViewBag.RateCodeId = ratecodeid;

            List<int> branchIds = new List<int>();

            string[] branchesSplit = branches.Split(',');
            foreach (string b in branchesSplit) {
                branchIds.Add(Convert.ToInt32(b));
            }

            RateUpdateViewModel viewModel = new RateUpdateViewModel();
            BranchesGetQuery bQuery = new BranchesGetQuery { Filter = new BranchFilterModel { Ids = branchIds } };
            viewModel.SelectedBranches = _query.Process(bQuery).Items;

            RateCodeGetByIdQuery rcQuery = new RateCodeGetByIdQuery { Id = ratecodeid };
            viewModel.RateCode = _query.Process(rcQuery);

            VehicleGroupsGetQuery vgQuery = new VehicleGroupsGetQuery { Filter = new VehicleGroupFilterModel { IsArchived = false } };
            viewModel.VehicleGroups = _query.Process(vgQuery).Items;


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add(string branches, int ratecodeid, RateUpdateViewModel viewModel, FormCollection fc) {

            VehicleGroupsGetQuery vgQuery = new VehicleGroupsGetQuery { Filter = new VehicleGroupFilterModel { IsArchived = false } };
            var groups = _query.Process(vgQuery).Items;
            List<Tuple<int, decimal>> ratesForGroups = new List<Tuple<int, decimal>>();


            foreach (var group in groups) {
                if (fc["vg_" + group.Id] != null) {
                    decimal groupRate = Convert.ToDecimal(fc["vg_" + group.Id.ToString()]);
                    ratesForGroups.Add(new Tuple<int, decimal>(group.Id, groupRate));
                }
            }

            RateAddCommand addRate = new RateAddCommand();
            branches.Split(',').ToList().ForEach(x => addRate.BranchIds.Add(Convert.ToInt32(x)));
            addRate.EndDate = viewModel.EndDate;
            addRate.StartDate = viewModel.StartDate;
            addRate.RateCodeId = ratecodeid;
            addRate.IsOpenEnded = viewModel.IsOpenEnded;
            addRate.RatesForGroups = ratesForGroups;

            _commandBus.Submit(addRate);

            return RedirectToAction("Index");
            //return View(viewModel);

        }

        public ActionResult Edit(string branches, int ratecodeid, bool isopenended, DateTime? startdate, DateTime? enddate) {

            ViewBag.Branches = branches;
            ViewBag.RateCodeId = ratecodeid;
            ViewBag.IsOpenEnded = isopenended;
            ViewBag.StartDate = startdate;
            ViewBag.EndDate = enddate;

            RateFilterModel filter = new RateFilterModel();
            branches.Split(',').ToList().ForEach(x => filter.BranchIds.Add(Convert.ToInt32(x)));
            filter.RateCodeId = ratecodeid;
            filter.IsOpenEnded = isopenended;
            if (!isopenended) {
                filter.ValidStartDate = startdate;
                filter.ValidEndDate = enddate;
            }

            var rateBranches = _query.Process(new BranchesGetQuery { Filter = new BranchFilterModel { Ids = filter.BranchIds }, Pagination = null }).Items;
            var rateCode = _query.Process(new RateCodeGetByIdQuery { Id = ratecodeid });
            var vehicleGroups = _query.Process(new VehicleGroupsGetQuery { Filter = new VehicleGroupFilterModel { IsArchived = false }, Pagination = null }).Items;


            RateDatesViewModel appliesTo = new RateDatesViewModel();
            appliesTo.IsOpenEnded = isopenended;
            appliesTo.ValidStartDate = startdate;
            appliesTo.ValidEndDate = enddate;

            RateUpdateViewModel viewModel = new RateUpdateViewModel();
            //RateSearchViewModel viewModel = new RateSearchViewModel();
            viewModel.SelectedBranches = rateBranches;
            viewModel.IsOpenEnded = isopenended;
            viewModel.StartDate = startdate;
            viewModel.EndDate = enddate;
            viewModel.RateCode = rateCode;
            viewModel.VehicleGroups = vehicleGroups;
            viewModel.RateAppliesTo = appliesTo;


            RateGetQuery ratesQuery = new RateGetQuery { Filter = filter, Pagination = null };
            var rates = _query.Process(ratesQuery).Items;

            viewModel.Rates = rates;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(RateUpdateViewModel viewModel, string branches, int ratecodeid, bool isopenended, DateTime? startdate, DateTime? enddate, FormCollection fc) {

            ViewBag.Branches = branches;
            ViewBag.RateCodeId = ratecodeid;
            ViewBag.IsOpenEnded = isopenended;
            ViewBag.StartDate = startdate;
            ViewBag.EndDate = enddate;

            VehicleGroupsGetQuery vgQuery = new VehicleGroupsGetQuery { Filter = new VehicleGroupFilterModel { IsArchived = false } };
            var groups = _query.Process(vgQuery).Items;
            List<Tuple<int, decimal>> ratesForGroups = new List<Tuple<int, decimal>>();

            

            return RedirectToAction("Index");
        }
    }
}