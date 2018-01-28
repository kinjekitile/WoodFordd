using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class PageContentRepository : RepositoryBase, IPageContentRepository {

        private const string ContentNotFound = "Content could not be found";

        public PageContentRepository(IDbConnectionConfig connection) : base(connection) { }

        public PageContentModel Create(PageContentModel model) {
            PageContent c = new Data.PageContent();
            c.PageTitle = model.PageTitle;
            c.MetaKeywords = model.MetaKeywords;
            c.MetaDescription = model.MetaDescription;
            c.PageContent1 = model.PageContent;
            c.DynamicPageId = model.DynamicPageId;
            c.VehicleGroupId = model.VehicleGroupId;
            c.VehicleId = model.VehicleId;
            c.NewsId = model.NewsId;
            c.CampaignId = model.CampaignId;
            c.BranchId = model.BranchId;
            c.VehicleManufacturerId = model.VehicleManufacturerId;
            _db.PageContents.Add(c);
            _db.SaveChanges();
            model.Id = c.Id;
            return model;
        }

        public PageContentModel GetByForeignKey(int id, PageContentForeignKey foreignKey) {
            var list = getAsIQueryable();
            PageContentModel res = null;
            switch (foreignKey) {
                case PageContentForeignKey.DynamicPageId:
                    res = list.Where(x => x.DynamicPageId == id).SingleOrDefault();
                    break;
                case PageContentForeignKey.VehicleGroupId:
                    res = list.Where(x => x.VehicleGroupId == id).SingleOrDefault();
                    break;
                case PageContentForeignKey.VehicleId:
                    res = list.Where(x => x.VehicleId == id).SingleOrDefault();
                    break;
                case PageContentForeignKey.NewsId:
                    res = list.Where(x => x.NewsId == id).SingleOrDefault();
                    break;
                case PageContentForeignKey.CampaignId:
                    res = list.Where(x => x.CampaignId == id).SingleOrDefault();
                    break;
                case PageContentForeignKey.BranchId:
                    res = list.Where(x => x.BranchId == id).SingleOrDefault();
                    break;
                case PageContentForeignKey.VehicleManufacturerId:
                    res = list.Where(x => x.VehicleManufacturerId == id).SingleOrDefault();
                    break;

            }
            return res;
        }

        public PageContentModel GetById(int id) {
            return getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
        }

        //public int? GetForeignKeyIdByUrl(string url, PageContentForeignKey foreignKey) {
        //    var list = getAsIQueryable().Where(x => x.PageUrl == url);
        //    int? foreignKeyId = null;
        //    switch (foreignKey) {
        //        case PageContentForeignKey.DynamicPageId:
        //            foreignKeyId = list.Select(x => x.DynamicPageId).SingleOrDefault();
        //            break;
        //        case PageContentForeignKey.VehicleGroupId:                    
        //            foreignKeyId = list.Select(x => x.VehicleGroupId).SingleOrDefault();
        //            break;
        //        case PageContentForeignKey.VehicleId:                    
        //            foreignKeyId = list.Select(x => x.VehicleId).SingleOrDefault();
        //            break;
        //        case PageContentForeignKey.NewsId:                    
        //            foreignKeyId = list.Select(x => x.NewsId).SingleOrDefault();
        //            break;
        //        case PageContentForeignKey.CampaignId:                    
        //            foreignKeyId = list.Select(x => x.CampaignId).SingleOrDefault();
        //            break;
        //        case PageContentForeignKey.BranchId:                    
        //            foreignKeyId = list.Select(x => x.BranchId).SingleOrDefault();
        //            break;
        //    }

        //    return foreignKeyId;
        //}

        //public int? GetIdByUrl(string url) {            
        //    int? id = getAsIQueryable().Where(x => x.PageUrl == url).Select(x => x.Id).SingleOrDefault();
        //    return id;
        //}

        public PageContentModel Update(PageContentModel model) {
            PageContent c = _db.PageContents.Where(x => x.Id == model.Id).SingleOrDefault();
            if (c == null)
                throw new Exception(ContentNotFound);
            c.PageTitle = model.PageTitle;
            c.MetaKeywords = model.MetaKeywords;
            c.MetaDescription = model.MetaDescription;
            c.PageContent1 = model.PageContent;
            _db.SaveChanges();
            return model;
        }

        private IQueryable<PageContentModel> getAsIQueryable() {
            return _db.PageContents.Select(x => new PageContentModel {
                Id = x.Id,
                PageTitle = x.PageTitle,
                MetaKeywords = x.MetaKeywords,
                MetaDescription = x.MetaDescription,
                PageContent = x.PageContent1,
                DynamicPageId = x.DynamicPageId,
                VehicleGroupId = x.VehicleGroupId,
                VehicleId = x.VehicleId,
                NewsId = x.NewsId,
                CampaignId = x.CampaignId,
                BranchId = x.BranchId,
                VehicleManufacturerId = x.VehicleManufacturerId
            });
        }
    }
}
