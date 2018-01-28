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
    public class UrlRedirectService : IUrlRedirectService {
        private readonly IUrlRedirectRepository _repo;
        public UrlRedirectService(IUrlRedirectRepository repo) {
            _repo = repo;
        }

        public UrlRedirectModel Create(UrlRedirectModel model) {
            return _repo.Create(model);
        }

        public void Delete(int id) {
            _repo.Delete(id);
        }

        public ListOf<UrlRedirectModel> Get(UrlRedirectFilterModel filter, ListPaginationModel pagination) {
            ListOf<UrlRedirectModel> res = new ListOf<UrlRedirectModel>();

            res.Pagination = pagination;
            res.Items = _repo.Get(filter, pagination);
            if (pagination != null) {
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }

            return res;
        }

        public UrlRedirectModel GetById(int id) {
            return _repo.GetById(id);
        }

        public UrlRedirectModel GetByUrl(string url) {
            return _repo.GetByUrl(url);
        }

        public UrlRedirectModel Update(UrlRedirectModel model) {
            return _repo.Update(model);
        }
    }
}
