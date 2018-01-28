using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class PageContentService : IPageContentService {

        private readonly IPageContentRepository _repo;
        public PageContentService(IPageContentRepository repo) {
            _repo = repo;
        }

        public PageContentModel Create(PageContentModel model) {
            return _repo.Create(model);
        }

        public PageContentModel GetByForeignKey(int id, PageContentForeignKey foreignKey) {
            return _repo.GetByForeignKey(id, foreignKey);
        }

        public PageContentModel GetById(int id) {
            return _repo.GetById(id);
        }

        //public int? GetForeignKeyIdByUrl(string url, PageContentForeignKey foreignKey) {
        //    return _repo.GetForeignKeyIdByUrl(url, foreignKey);
        //}

        //public int? GetIdByUrl(string url) {
        //    return _repo.GetIdByUrl(url);
        //}

        public PageContentModel Update(PageContentModel model) {
            return _repo.Update(model);
        }
    }
}
