using FluentValidation.Mvc;
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
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator")]
    public class RateUpdateController : Controller {

        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public RateUpdateController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Update() {

            RateSearchAndUpdateViewModel model = new RateSearchAndUpdateViewModel();
            model.Search = new RateSearchModel();
            //RateSearchQuery query = new RateSearchQuery { SearchModel = model.Search };
            //model.Results = _query.Process(query);
            //model.Results.Rates = null;
            populateBranchCheckboxList(model, new List<int>().ToArray());
            ViewBag.Success = false;

            return View(model);
        }

        public ActionResult Filter() {
            return RedirectToAction("Update");
        }

        [HttpPost]
        public ActionResult Filter([CustomizeValidator(RuleSet = RateUpdateFilterValidationRuleSets.Default)] RateSearchAndUpdateViewModel model) {

            //needed for binding the selectedbranches from the FindRates route
            //if (!string.IsNullOrEmpty(model.SelectedBranchIdsString)) {
            //    string[] bids = model.SelectedBranchIdsString.Split(',');
            //    List<int> bidInts = new List<int>();
            //    foreach (string b in bids) {
            //        bidInts.Add(Convert.ToInt32(b));
            //    }
            //    model.SelectedBranchIds = bidInts.ToArray();
            //}
            populateBranchListFromCommaSeperatedString(model);
            populateBranchCheckboxList(model, model.SelectedBranchIds);

            if (model.SelectedBranchIds != null) {
                model.SelectedBranchIdsString = String.Join(",", model.SelectedBranchIds);
            }


            if (ModelState.IsValid) {


                RateSearchQuery query = new RateSearchQuery { SearchModel = model.Search };
                model.Results = _query.Process(query);

                model.HasSearchRun = true;
                model.UpdateDateRange = new RateDatesViewModel { IsOpenEnded = model.Search.IsOpenEnded, ValidStartDate = model.Search.StartDate, ValidEndDate = model.Search.EndDate };

            }


            ViewBag.Success = false;
            return View("Update", model);
        }

        [HttpPost]
        public ActionResult Update(RateSearchAndUpdateViewModel viewModel, int rateCodeId, FormCollection fc) {
            ViewBag.Success = false;
            //i hate checkbox lists
            populateBranchListFromCommaSeperatedString(viewModel);
            populateBranchCheckboxList(viewModel, viewModel.SelectedBranchIds);

            bool isOpenEnded = viewModel.UpdateDateRange.IsOpenEnded;
            DateTime? startDate = viewModel.UpdateDateRange.ValidStartDate;
            DateTime? endDate = viewModel.UpdateDateRange.ValidEndDate;


            var branches = _query.Process(new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false }, Pagination = null }).Items;
            var vehicleGroups = _query.Process(new VehicleGroupsGetQuery { Filter = new VehicleGroupFilterModel { IsArchived = false }, Pagination = null }).Items;

            List<RateModel> newRates = new List<RateModel>();
            List<RateModel> existingRates = new List<RateModel>();
            List<RateModel> removedRates = new List<RateModel>();

            //List<int> selectedBranchIds = new List<int>();
            bool validationFailed = false;
            foreach (var vg in vehicleGroups) {
                foreach (var b in branches) {
                    string formId = "vehicleBranch_" + vg.Id + "_" + b.Id;
                    string formIdrate = formId + "_rateid";
                    string formIdPerKm = formId + "_perkm";
                    string formIdFreeKms = formId + "_freekms";
                    string formIdIsUnlimited = formId + "_isunlimited";
                    //selectedBranchIds.Add(b.Id);
                    RateModel rate = new RateModel();
                    if (!string.IsNullOrEmpty(fc[formId])) {

                        rate.BranchId = b.Id;

                        rate.VehicleGroupId = vg.Id;
                        rate.RateCodeId = rateCodeId;

                        try {
                            rate.Price = Convert.ToDecimal(fc[formId]);

                            if (string.IsNullOrEmpty(fc[formIdPerKm])) {
                                rate.CostPerKm = 0;
                            } else {
                                rate.CostPerKm = Convert.ToDecimal(fc[formIdPerKm]);
                            }

                            if (fc[formIdFreeKms] == "ulm") {
                                rate.HasUnlimitedKms = true;
                            } else {
                                rate.HasUnlimitedKms = false;
                            }

                            if (!rate.HasUnlimitedKms) {
                                if (string.IsNullOrEmpty(fc[formIdFreeKms])) {
                                    rate.FreeKms = 0;
                                } else {
                                    rate.FreeKms = Convert.ToInt32(fc[formIdFreeKms]);
                                }

                            }


                        } catch (Exception) {

                            validationFailed = true;
                            break;
                        }

                        rate.IsOpenEnded = isOpenEnded;

                        if (rate.IsOpenEnded) {
                            rate.ValidStartDate = null;
                            rate.ValidEndDate = null;
                        } else {
                            rate.ValidStartDate = startDate;
                            rate.ValidEndDate = endDate;
                        }

                        if (fc[formIdrate] != null) {

                            //Existing rate
                            rate.Id = Convert.ToInt32(fc[formIdrate]);
                            existingRates.Add(rate);

                        } else {
                            newRates.Add(rate);
                        }
                    } else {
                        //No price set, if rate exists then delete it
                        rate.Id = Convert.ToInt32(fc[formIdrate]);
                        if (rate.Id > 0) {
                            removedRates.Add(rate);
                        }
                    }
                }
                if (validationFailed) {
                    break;
                }
            }

            if (validationFailed) {
                ModelState.AddModelError("", "Rate or Per Km value(s) are invalid");
                ViewBag.Success = false;
            } else {
                RateUpsertCommand upsertCommand = new RateUpsertCommand();
                upsertCommand.ExistingRates = existingRates;
                upsertCommand.NewRates = newRates;
                upsertCommand.RemoveRates = removedRates;

                _commandBus.Submit(upsertCommand);

                ModelState.Clear();

                viewModel.Search.StartDate = viewModel.UpdateDateRange.ValidStartDate;
                viewModel.Search.EndDate = viewModel.UpdateDateRange.ValidEndDate;
                viewModel.Search.IsOpenEnded = viewModel.UpdateDateRange.IsOpenEnded;
                ViewBag.Success = true;
            }

            //viewModel.SelectedBranchIds = selectedBranchIds.ToArray();

            populateBranchCheckboxList(viewModel, viewModel.SelectedBranchIds);
            RateSearchQuery query = new RateSearchQuery { SearchModel = viewModel.Search };
            viewModel.Results = _query.Process(query);
            viewModel.HasSearchRun = true;
            //viewModel.UpdateDateRange = new RateDatesViewModel { IsOpenEnded = isOpenEnded, ValidStartDate = startDate, ValidEndDate = endDate };

            //reset filter


            //viewModel.Search.IsOpenEnded = isOpenEnded;
            //viewModel.Search.StartDate = startDate;
            //viewModel.Search.EndDate = endDate;



            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteFilter([CustomizeValidator(RuleSet = RateUpdateFilterValidationRuleSets.Default)] RateSearchAndUpdateViewModel model) {

            //needed for binding the selectedbranches from the FindRates route
            //if (!string.IsNullOrEmpty(model.SelectedBranchIdsString)) {
            //    string[] bids = model.SelectedBranchIdsString.Split(',');
            //    List<int> bidInts = new List<int>();
            //    foreach (string b in bids) {
            //        bidInts.Add(Convert.ToInt32(b));
            //    }
            //    model.SelectedBranchIds = bidInts.ToArray();
            //}
            populateBranchListFromCommaSeperatedString(model);
            populateBranchCheckboxList(model, model.SelectedBranchIds);

            if (model.SelectedBranchIds != null) {
                model.SelectedBranchIdsString = String.Join(",", model.SelectedBranchIds);
            }


            if (ModelState.IsValid) {


                RateSearchQuery query = new RateSearchQuery { SearchModel = model.Search };
                model.Results = _query.Process(query);

                model.HasSearchRun = true;
                model.UpdateDateRange = new RateDatesViewModel { IsOpenEnded = model.Search.IsOpenEnded, ValidStartDate = model.Search.StartDate, ValidEndDate = model.Search.EndDate };

            }


            ViewBag.Success = false;
            return View("Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(RateSearchAndUpdateViewModel viewModel, int rateCodeId, FormCollection fc) {
            ViewBag.Success = false;
            //i hate checkbox lists
            populateBranchListFromCommaSeperatedString(viewModel);
            populateBranchCheckboxList(viewModel, viewModel.SelectedBranchIds);

            bool isOpenEnded = viewModel.UpdateDateRange.IsOpenEnded;
            DateTime? startDate = viewModel.UpdateDateRange.ValidStartDate;
            DateTime? endDate = viewModel.UpdateDateRange.ValidEndDate;


            var branches = _query.Process(new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false }, Pagination = null }).Items;
            var vehicleGroups = _query.Process(new VehicleGroupsGetQuery { Filter = new VehicleGroupFilterModel { IsArchived = false }, Pagination = null }).Items;

            List<RateModel> removedRates = new List<RateModel>();

            //List<int> selectedBranchIds = new List<int>();
            bool validationFailed = false;
            foreach (var vg in vehicleGroups) {
                foreach (var b in branches) {
                    string formId = "vehicleBranch_" + vg.Id + "_" + b.Id;
                    string formIdrate = formId + "_rateid";
                    string formIdPerKm = formId + "_perkm";
                    string formIdFreeKms = formId + "_freekms";
                    string formIdIsUnlimited = formId + "_isunlimited";
                    //selectedBranchIds.Add(b.Id);
                    RateModel rate = new RateModel();



                    if (fc[formIdrate] != null) {

                        //Existing rate
                        rate.Id = Convert.ToInt32(fc[formIdrate]);
                        removedRates.Add(rate);

                    }


                }

            }


            RateUpsertCommand upsertCommand = new RateUpsertCommand();
            upsertCommand.ExistingRates = new List<RateModel>();
            upsertCommand.NewRates = new List<RateModel>();
            upsertCommand.RemoveRates = removedRates;

            _commandBus.Submit(upsertCommand);



            return RedirectToAction("Deleted");
        }

        public ActionResult Deleted() {
            return View();
        }

        private void populateBranchListFromCommaSeperatedString(RateSearchAndUpdateViewModel model) {
            //needed for binding the selectedbranches from the FindRates route
            if (!string.IsNullOrEmpty(model.SelectedBranchIdsString)) {
                string[] bids = model.SelectedBranchIdsString.Split(',');
                List<int> bidInts = new List<int>();
                foreach (string b in bids) {
                    bidInts.Add(Convert.ToInt32(b));
                }
                model.SelectedBranchIds = bidInts.ToArray();
            }
        }

        private void populateBranchCheckboxList(RateSearchAndUpdateViewModel model, int[] branchIds) {
            BranchesGetQuery queryBranches = new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false } };
            ListOf<BranchModel> branches = _query.Process(queryBranches);

            model.AllBranches = branches.Items;
            model.SelectedBranchIds = branchIds;
            if (model.SelectedBranchIds != null) {
                model.SelectedBranches = branches.Items.Where(x => model.SelectedBranchIds.Contains(x.Id)).ToList();
            } else {
                model.SelectedBranches = new List<BranchModel>();
            }

            if (branchIds != null) {
                model.Search.BranchIds = branchIds.ToList();
            }


        }
    }
}