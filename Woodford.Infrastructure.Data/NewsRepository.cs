using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class NewsCategoryRepository : RepositoryBase, INewsCategoryRepository {

        private const string ItemNotFound = "News Category could not be found";

        public NewsCategoryRepository(IDbConnectionConfig connection) : base(connection) { }

        public NewsCategoryModel Create(NewsCategoryModel model) {
            NewsCategory c = new NewsCategory();
            c.Title = model.Title;
            c.IsArchived = false;
            c.PageUrl = model.PageUrl;
            c.PageTitle = model.PageTitle;
            c.MetaDescription = model.MetaDescription;
            c.MetaKeywords = model.MetaKeywords;

            _db.NewsCategories.Add(c);
            _db.SaveChanges();
            model.Id = c.Id;
            return model;
        }

        public NewsCategoryModel Update(NewsCategoryModel model) {

            NewsCategory c = _db.NewsCategories.Where(x => x.Id == model.Id).SingleOrDefault();
            if (c == null)
                throw new Exception(ItemNotFound);
            c.Title = model.Title;
            c.IsArchived = model.IsArchived;
            c.PageUrl = model.PageUrl;
            c.PageTitle = model.PageTitle;
            c.MetaDescription = model.MetaDescription;
            c.MetaKeywords = model.MetaKeywords;
            _db.SaveChanges();

            return model;
        }

        public List<NewsCategoryModel> Get(NewsCategoryFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.IsArchived.HasValue)
                    list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public int GetCount(NewsCategoryFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.IsArchived.HasValue)
                    list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
            }
            return list.Count();
        }

        public NewsCategoryModel GetById(int id) {
            NewsCategoryModel c = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (c == null)
                throw new Exception(ItemNotFound);
            return c;
        }

        private IQueryable<NewsCategoryModel> getAsIQueryable() {
            return _db.NewsCategories.Select(x => new NewsCategoryModel {
                Id = x.Id,
                Title = x.Title,
                IsArchived = x.IsArchived,
                PageUrl = x.PageUrl,
                PageTitle = x.PageTitle,
                MetaDescription = x.MetaDescription,
                MetaKeywords = x.MetaKeywords
            });
        }

        public NewsCategoryModel GetByUrl(string url) {
            NewsCategoryModel c = getAsIQueryable().Where(x => x.PageUrl == url).SingleOrDefault();
            if (c == null)
                throw new Exception(ItemNotFound);
            return c;
        }
    }

    public class NewsRepository : RepositoryBase, INewsRepository {

        private const string ItemNotFound = "News Article could not be found";

        public NewsRepository(IDbConnectionConfig connection) : base(connection) { }

        public NewsModel Create(NewsModel model) {
            News a = new News();
            a.Headline = model.Headline;
            a.SubHeadline = model.SubHeadline;
            a.ShortDescription = model.ShortDescription;
            a.FileUploadId = model.FileUploadId;
            a.DateCreated = DateTime.Now;
            a.Author = model.Author;
            a.IsArchived = false;
            a.NewsCategoryId = model.NewsCategoryId;
            a.PageUrl = model.PageUrl;

            _db.News.Add(a);
            _db.SaveChanges();
            model.Id = a.Id;
            return model;
        }

        public List<NewsModel> Get(NewsFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.NewsCategoryId.HasValue)
                    list = list.Where(x => x.NewsCategoryId == filter.NewsCategoryId.Value);
                if (filter.IsArchived.HasValue)
                    list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public NewsModel GetById(int id) {
            NewsModel a = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (a == null)
                throw new Exception(ItemNotFound);
            return a;
        }

        public NewsModel GetByUrl(string url) {
            NewsModel a = getAsIQueryable().Where(x => x.PageUrl == url).OrderByDescending(x => x.DateCreated).FirstOrDefault();
            if (a == null)
                throw new Exception(ItemNotFound);
            return a;
        }

        public int GetCount(NewsFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.NewsCategoryId.HasValue)
                    list = list.Where(x => x.NewsCategoryId == filter.NewsCategoryId.Value);
                if (filter.IsArchived.HasValue)
                    list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
            }
            return list.Count();
        }

        public NewsModel Update(NewsModel model) {
            News a = _db.News.Where(x => x.Id == model.Id).SingleOrDefault();
            if (a == null)
                throw new Exception(ItemNotFound);
            a.Headline = model.Headline;
            a.SubHeadline = model.SubHeadline;
            a.ShortDescription = model.ShortDescription;
            a.FileUploadId = model.FileUploadId;
            a.Author = model.Author;
            a.IsArchived = model.IsArchived;
            a.NewsCategoryId = model.NewsCategoryId;
            a.PageUrl = model.PageUrl;

            _db.SaveChanges();
            model.Id = a.Id;
            return model;
        }

        private IQueryable<NewsModel> getAsIQueryable() {
            return _db.News.Select(x => new NewsModel {
                Id = x.Id,
                Headline = x.Headline,
                SubHeadline = x.SubHeadline,
                ShortDescription = x.ShortDescription,
                FileUploadId = x.FileUploadId,
                NewsImage = x.FileUploadId.HasValue ? (new FileUploadModel {
                    Id = x.FileUpload.Id,
                    Title = x.FileUpload.Title,
                    FileExtension = x.FileUpload.FileExtension,
                    DateUploaded = x.FileUpload.DateUploaded
                }) : null,
                Author = x.Author,
                IsArchived = x.IsArchived,
                NewsCategoryId = x.NewsCategoryId,
                PageUrl = x.PageUrl,
                DateCreated = x.DateCreated
            });
        }
    }
}
