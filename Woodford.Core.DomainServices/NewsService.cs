using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class NewsCategoryService : INewsCategoryService {
        private readonly INewsCategoryRepository _repo;
        private readonly INewsService _newsService;
        public NewsCategoryService(INewsCategoryRepository repo, INewsService newsService) {
            _repo = repo;
            _newsService = newsService;
        }
        public NewsCategoryModel Create(NewsCategoryModel model) {
            return _repo.Create(model);            
        }

        public ListOf<NewsCategoryModel> Get(NewsCategoryFilterModel filter, ListPaginationModel pagination) {

            ListOf<NewsCategoryModel> res = new ListOf<NewsCategoryModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;
        }

        public NewsCategoryModel GetById(int id, bool includeArticles = false, ListPaginationModel pagination = null) {
            NewsCategoryModel c = _repo.GetById(id);            
            if (includeArticles) {
                c.Articles = _newsService.Get(new NewsFilterModel { NewsCategoryId = id, IsArchived = false }, pagination);
            }
            return c;
        }

        public NewsCategoryModel Update(NewsCategoryModel model) {
            return _repo.Update(model);
        }

        public NewsCategoryModel GetByUrl(string url, bool includeArticles = false, ListPaginationModel pagination = null) {
            NewsCategoryModel c = _repo.GetByUrl(url);
            if (includeArticles) {
                c.Articles = _newsService.Get(new NewsFilterModel { NewsCategoryId = c.Id, IsArchived = false }, pagination);
            }
            return c;
        }

        public NewsCategoryModel MarkAs(int id, bool markAs) {
            NewsCategoryModel c = _repo.GetById(id);
            c.IsArchived = markAs;
            return _repo.Update(c);
        }
    }

    public class NewsService : INewsService {
        private readonly INewsRepository _repo;
        private readonly IFileUploadService _fileUploads;
        private readonly IPageContentService _pageContentService;

        public NewsService(INewsRepository repo, IFileUploadService fileUploads, IPageContentService pageContentService) {
            _repo = repo;
            _fileUploads = fileUploads;
            _pageContentService = pageContentService;
        }

        public NewsModel Create(NewsModel model) {
            if (model.NewsImage != null) {
                FileUploadModel f = _fileUploads.Create(model.NewsImage);
                model.FileUploadId = f.Id;
            }

            NewsModel a = _repo.Create(model);
            a.PageContent.NewsId = a.Id;
            a.PageContent = _pageContentService.Create(a.PageContent);
            return a;
        }

        public ListOf<NewsModel> Get(NewsFilterModel filter, ListPaginationModel pagination) {
            ListOf<NewsModel> res = new ListOf<NewsModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;
        }

        public NewsModel GetById(int id, bool includePageContent = false) {
            NewsModel a = _repo.GetById(id);
            if (includePageContent) {
                a.PageContent = _pageContentService.GetByForeignKey(a.Id, PageContentForeignKey.NewsId);
            }
            return a;
        }

        public NewsModel GetByUrl(string url, bool includePageContent = false) {
            NewsModel a = _repo.GetByUrl(url);
            if (includePageContent) {
                a.PageContent = _pageContentService.GetByForeignKey(a.Id, PageContentForeignKey.NewsId);
            }
            return a;
        }

        public NewsModel Update(NewsModel model) {
            if (model.NewsImage != null) {
                if (model.NewsImage.FileContents != null) {
                    if (model.FileUploadId.HasValue) {
                        model.NewsImage.Id = model.FileUploadId.Value;
                        model.FileUploadId = _fileUploads.Update(model.NewsImage).Id;
                    } else {
                        FileUploadModel f = _fileUploads.Create(model.NewsImage);
                        model.FileUploadId = f.Id;
                    }
                }
            }
            NewsModel a = _repo.Update(model);
            _pageContentService.Update(model.PageContent);

            return a;
        }

        public NewsModel MarkAs(int id, bool markAs) {
            NewsModel a = _repo.GetById(id);
            a.IsArchived = markAs;
            return _repo.Update(a);
        }
    }
}
