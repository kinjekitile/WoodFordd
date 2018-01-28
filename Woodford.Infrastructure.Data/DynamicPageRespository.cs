using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class DynamicPageRespository : RepositoryBase, IDynamicPageRepository {

        private const string PageNotFound = "Page could not be found";

        public DynamicPageRespository(IDbConnectionConfig connection) : base(connection) { }
    
        public DynamicPageModel Create(DynamicPageModel model) {
            DynamicPage d = new DynamicPage();
            d.AdminDescription = model.AdminDescription;
            d.IsArchived = model.IsArchived;
            d.PageUrl = model.PageUrl;            

            _db.DynamicPages.Add(d);
            _db.SaveChanges();
            model.Id = d.Id;
            return model;
        }

        public List<DynamicPageModel> Get(DynamicPageFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue) list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.IsArchived.HasValue) list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
            
        }

        public DynamicPageModel GetById(int id) {
            DynamicPageModel d = getAsIQueryable().Where(x => x.Id == id && !x.IsArchived).FirstOrDefault();            
            if (d == null)
                throw new Exception(PageNotFound);
            return d;
        }

        public int GetCount(DynamicPageFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue) list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.IsArchived.HasValue) list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
            }
            return list.Count();
        }

        public int? GetIdByUrl(string url) {
            return getAsIQueryable().Where(x => x.PageUrl == url && !x.IsArchived).Select(x => x.Id).FirstOrDefault();
        }

        public DynamicPageModel Update(DynamicPageModel model) {
            DynamicPage d = _db.DynamicPages.Where(x => x.Id == model.Id).SingleOrDefault();
            if (d == null)
                throw new Exception(PageNotFound);

            d.AdminDescription = model.AdminDescription;
            d.IsArchived = model.IsArchived;
            d.PageUrl = model.PageUrl;
           
            _db.SaveChanges();            
            return model;
        }

        private IQueryable<DynamicPageModel> getAsIQueryable() {
            return _db.DynamicPages.Select(x => new DynamicPageModel {
                Id = x.Id,
                AdminDescription = x.AdminDescription,
                IsArchived = x.IsArchived,
                PageUrl = x.PageUrl       
            });
        }        
    }
}
