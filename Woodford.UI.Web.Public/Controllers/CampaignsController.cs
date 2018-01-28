using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.Code;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers
{
    public class CampaignsController : Controller
    {
        private IQueryProcessor _query;

        public CampaignsController(IQueryProcessor query) {
            _query = query;
        }

        [Route("campaigns")]
        public ActionResult Index()
        {
            CampaignGetQuery query = new CampaignGetQuery { Filter = new CampaignFilterModel {  IsArchived = false } };

            var results = _query.Process(query);
            results.Items = results.Items.Where(x => x.EndDate >= DateTime.Today && x.StartDate <= DateTime.Today).ToList();
            if (results.Items.Count == 1) {
                return RedirectToAction("Campaign", new { campaignUrl = results.Items[0].PageUrl });
            }
            return View(results);
        }

        [Route("campaigns/{campaignUrl?}")]
        public ActionResult Campaign(string campaignUrl) {

            CampaignGetByUrlQuery query = new CampaignGetByUrlQuery { Url = campaignUrl, IncludePageContent = true };
            CampaignModel c = _query.Process(query);

            CampaignViewModel viewModel = new CampaignViewModel();
            viewModel.Campaign = c;
            viewModel.Search = new SearchResultsViewModel();
            viewModel.Search.Results = new SearchResultsModel();
            viewModel.Search.Criteria = SearchCriteriaViewModelFactory.CreateInstance(c.Id);
            viewModel.Search.Results.Criteria = viewModel.Search.Criteria.Criteria;

            viewModel.Search.Criteria.Criteria.CampaignId = c.Id;

            BookingSearchQuery querySearch = new BookingSearchQuery { Criteria = viewModel.Search.Criteria.Criteria };
            viewModel.Search.Results = _query.Process(querySearch);



            return View(viewModel);
        }

        public ActionResult Search() {
            return View();
        }
    }
}