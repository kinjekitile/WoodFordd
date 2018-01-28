using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class UrlRedirectRepository : RepositoryBase, IUrlRedirectRepository {

        private const string NotFound = "Url Redirect not found";
        public UrlRedirectRepository(IDbConnectionConfig connection) : base(connection) { }

        public UrlRedirectModel Create(UrlRedirectModel model) {
            UrlRedirect r = new UrlRedirect();
            r.OldUrl = model.OldUrl;
            r.NewUrl = model.NewUrl;
            switch (model.RedirectType) {
                case UrlRedirectType.MovedPermanently:
                    r.HttpStatusCode = "301";
                    break;
                case UrlRedirectType.NotFound:
                    r.HttpStatusCode = "404";
                    break;
                case UrlRedirectType.Gone:
                    r.HttpStatusCode = "410";
                    break;
            }

            _db.UrlRedirects.Add(r);
            _db.SaveChanges();
            model.Id = r.Id;
            return model;
        }

        public void Delete(int id) {
            UrlRedirect r = _db.UrlRedirects.SingleOrDefault(x => x.Id == id);
            if (r == null) {
                throw new Exception(NotFound);
            }
            _db.UrlRedirects.Remove(r);
            _db.SaveChanges();
        }

        public List<UrlRedirectModel> Get(UrlRedirectFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public UrlRedirectModel GetById(int id) {
            UrlRedirectModel r = getAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (r == null) {
                throw new Exception(NotFound);
            }
            r = mapType(r);
            return r;
        }

        public UrlRedirectModel GetByUrl(string url) {
            UrlRedirectModel r = getAsIQueryable().SingleOrDefault(x => x.OldUrl.ToLower() == url.ToLower());
            if (r == null) {
                return r;
            }
            r = mapType(r);
            return r;
        }

        public int GetCount(UrlRedirectFilterModel filter) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public UrlRedirectModel Update(UrlRedirectModel model) {
            UrlRedirect r = _db.UrlRedirects.SingleOrDefault(x => x.Id == model.Id);
            if (r == null) {
                throw new Exception(NotFound);
            }
            r.OldUrl = model.OldUrl;
            r.NewUrl = model.NewUrl;
            switch (model.RedirectType) {
                case UrlRedirectType.MovedPermanently:
                    r.HttpStatusCode = "301";
                    break;
                case UrlRedirectType.NotFound:
                    r.HttpStatusCode = "404";
                    break;
                case UrlRedirectType.Gone:
                    r.HttpStatusCode = "410";
                    break;
            }
            _db.SaveChanges();

            return model;
        }
        private UrlRedirectModel mapType(UrlRedirectModel model) {

            switch (model.StatusCode) {
                case "301":
                    model.RedirectType = UrlRedirectType.MovedPermanently;
                    break;
                case "404":
                    model.RedirectType = UrlRedirectType.Gone;
                    break;
                case "410":
                    model.RedirectType = UrlRedirectType.NotFound;
                    break;
            }

            return model;
        }

        private IQueryable<UrlRedirectModel> applyFilter(IQueryable<UrlRedirectModel> list, UrlRedirectFilterModel filter) {

            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.RedirectType.HasValue)
                    list = list.Where(x => x.RedirectType == filter.RedirectType.Value);
                if (!string.IsNullOrEmpty(filter.Url))
                    list = list.Where(x => x.OldUrl.Contains(filter.Url) || x.NewUrl.Contains(filter.Url));
            }
            return list;
        }

        private IQueryable<UrlRedirectModel> getAsIQueryable() {
            return _db.UrlRedirects.Select(x => new UrlRedirectModel {
                Id = x.Id,
                OldUrl = x.OldUrl,
                NewUrl = x.NewUrl,
                StatusCode = x.HttpStatusCode
            });
        }
    }
}
