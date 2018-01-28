using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class FindRatesController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public FindRatesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        public ActionResult Index()
        {
            RateFindViewModel model = new RateFindViewModel();
            populateBranchCheckboxList(model, new List<int>().ToArray());
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(RateFindViewModel model) {
            populateBranchCheckboxList(model, model.SelectedBranchIds);
            if (ModelState.IsValid) {
                model.SearchRun = true;
                RateFindDatesQuery query = new RateFindDatesQuery { BranchIds = model.SelectedBranches.Select(x => x.Id).ToList(), RateCodeId = model.RateCodeId };
                model.Results = _query.Process(query);
            }
            return View(model);
        }

        private void populateBranchCheckboxList(RateFindViewModel model, int[] branchIds) {
            BranchesGetQuery queryBranches = new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false } };
            ListOf<BranchModel> branches = _query.Process(queryBranches);

            model.AllBranches = branches.Items;
            model.SelectedBranchIds = branchIds;
            model.SelectedBranches = branches.Items.Where(x => model.SelectedBranchIds.Contains(x.Id)).ToList();
            //model.BranchIds = branchIds.ToList();

        }
    }
}