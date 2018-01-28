using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class DynamicPageService : IDynamicPageService {
        private readonly IDynamicPageRepository _repo;
        private readonly IPageContentService _pageContentService;      
        public DynamicPageService(IDynamicPageRepository repo, IPageContentService pageContentService) {
            _repo = repo;
            _pageContentService = pageContentService;
        }
        public DynamicPageModel Create(DynamicPageModel model) {
            DynamicPageModel d = _repo.Create(model);
            d.PageContent.DynamicPageId = d.Id;
            d.PageContent = _pageContentService.Create(model.PageContent);
            return d;
        }

        public ListOfDynamicPageModel Get(DynamicPageFilterModel filter, ListPaginationModel pagination) {

            ListOfDynamicPageModel res = new ListOfDynamicPageModel();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Pages = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);                
                res.Pages = _repo.Get(filter, res.Pagination);
            }

            return res;
        }

        public DynamicPageModel GetById(int id, bool includePageContent) {

            DynamicPageModel d = _repo.GetById(id);
            if (includePageContent) {
                d.PageContent = _pageContentService.GetByForeignKey(d.Id, PageContentForeignKey.DynamicPageId);
            }

            return d;
        }

        public DynamicPageModel Update(DynamicPageModel model) {            
            DynamicPageModel d = _repo.Update(model);
            _pageContentService.Update(model.PageContent);
            return d;
        }

        public DynamicPageModel Archive(int id) {
            DynamicPageModel d = GetById(id, false);
            d.IsArchived = true;
            _repo.Update(d);
            return d;
        }

        public DynamicPageModel UnArchive(int id) {
            DynamicPageModel d = GetById(id, false);
            d.IsArchived = false;
            _repo.Update(d);
            return d;
        }

        public int? GetIdByUrl(string url) {
            return _repo.GetIdByUrl(url);
        }


        //public DynamicPageModel GetByUrl(string url, bool includePageContent = false) {
        //    return _repo.GetByUrl(url, includePageContent);
        //}
    }
}
