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
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator,SEO")]
    public class NewsController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public NewsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index(int categoryId, int p = 1) {
            NewsGetQuery query = new NewsGetQuery();
            query.Filter = new NewsFilterModel { NewsCategoryId = categoryId };
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

            NewsCategoryGetByIdQuery catQuery = new NewsCategoryGetByIdQuery { Id = categoryId, IncludeArticles = false, Pagination = null };

            NewsListViewModel model = new NewsListViewModel();
            model.Articles = _query.Process(query);
            model.NewsCategory = _query.Process(catQuery);
                        
            return View(model);
        }

        public ActionResult Add(int? categoryId) {
            NewsViewModel model = new NewsViewModel();
            model.News = new NewsModel();
            if (categoryId.HasValue) {
                NewsCategoryGetByIdQuery catQuery = new NewsCategoryGetByIdQuery { Id = categoryId.Value, IncludeArticles = false, Pagination = null };
                model.NewsCategory = _query.Process(catQuery);
                
                model.News.NewsCategoryId = categoryId.Value;
            } 
            
            
            
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add([CustomizeValidator(RuleSet = NewsValidationRuleSets.Default)] NewsViewModel model) {
            if (ModelState.IsValid) {

                model.News.NewsImage = FileUploadModelFactory.CreateInstance(model);
                //model.News.NewsCategoryId = model.NewsCategory.Id;
                NewsAddCommand command = new NewsAddCommand { Model = model.News };
                _commandBus.Submit(command);
                return RedirectToAction("Index", new { categoryId = model.NewsCategory.Id });
            } else {
                //NewsCategoryGetByIdQuery catQuery = new NewsCategoryGetByIdQuery { Id = model.NewsCategory.Id, IncludeArticles = false, Pagination = null };
                //model.NewsCategory = _query.Process(catQuery);
            }
            return View(model);
        }

        public ActionResult Edit(int id) {

            NewsViewModel model = new NewsViewModel();

            NewsGetByIdQuery query = new NewsGetByIdQuery { Id = id, IncludePageContent = true };
            model.News = _query.Process(query);

            NewsCategoryGetByIdQuery catQuery = new NewsCategoryGetByIdQuery { Id = model.News.NewsCategoryId, IncludeArticles = false, Pagination = null };
            model.NewsCategory = _query.Process(catQuery);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([CustomizeValidator(RuleSet = NewsValidationRuleSets.Default)] NewsViewModel model) {
            if (ModelState.IsValid) {

                model.News.NewsImage = FileUploadModelFactory.CreateInstance(model);
                //model.News.NewsCategoryId = model.NewsCategory.Id;
                NewsEditCommand command = new NewsEditCommand { Model = model.News };
                _commandBus.Submit(command);
                return RedirectToAction("Index", new { categoryId = model.NewsCategory.Id });
            } else {
                NewsGetByIdQuery query = new NewsGetByIdQuery { Id = model.News.Id, IncludePageContent = true };
                NewsModel a = _query.Process(query);               
                model.News.NewsImage = a.NewsImage;

                //NewsCategoryGetByIdQuery catQuery = new NewsCategoryGetByIdQuery { Id = model.NewsCategory.Id, IncludeArticles = false, Pagination = null };
                //model.NewsCategory = _query.Process(catQuery);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult MarkAsArchived(int id, int categoryId, int p) {
            NewsMarkAsCommand command = new NewsMarkAsCommand { Id = id, MarkAsArchived = true };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p, categoryId = categoryId });
        }

        [HttpPost]
        public ActionResult MarkAsActive(int id, int categoryId, int p = 1) {
            NewsMarkAsCommand command = new NewsMarkAsCommand { Id = id, MarkAsArchived = false };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p, categoryId = categoryId });
        }
    }
}