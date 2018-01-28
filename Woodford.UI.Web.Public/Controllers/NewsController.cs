using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Controllers
{
    public class NewsController : Controller
    {

        private IQueryProcessor _query;
        private ICommandBus _commandBus;

        public NewsController(IQueryProcessor query, ICommandBus commandBus) {
            _query = query;
            _commandBus = commandBus;
        }

        [Route("news")]
        public ActionResult Index()
        {            
            return View(getNewsCategories());
        }

        [Route("news/{categoryUrl?}")]
        
        public ActionResult Category(string categoryUrl) {
            return View(getNewsCategoryByUrl(categoryUrl));
        }

        [Route("news/article/{articleUrl?}")]
        public ActionResult Article(string articleUrl) {
            return View(getNewsByUrl(articleUrl));
        }

        private List<NewsCategoryModel> getNewsCategories() {
            NewsCategoryGetQuery query = new NewsCategoryGetQuery { Filter = new NewsCategoryFilterModel { IsArchived = false } };
            ListOf<NewsCategoryModel> cats = _query.Process(query);
            return cats.Items;
        }

        private NewsCategoryModel getNewsCategoryByUrl(string url) {
            NewsCategoryGetByUrlQuery query = new NewsCategoryGetByUrlQuery { Url = url, IncludeArticles = true };
            return _query.Process(query);
        }

        private NewsModel getNewsByUrl(string url) {
            NewsGetByUrlQuery query = new NewsGetByUrlQuery { Url = url, IncludePageContent = true };
            return _query.Process(query);
        }
    }
}