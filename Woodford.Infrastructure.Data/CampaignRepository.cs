using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class CampaignRepository : RepositoryBase, ICampaignRepository {
        private readonly string CampaignNotFound = "Campaign could not be found";

        public CampaignRepository(IDbConnectionConfig connection) : base(connection) { }

        public CampaignModel Create(CampaignModel model) {
            Campaign c = new Campaign();
            c.Title = model.Title;
            c.Description = model.Description;
            c.StartDate = model.StartDate;
            c.EndDate = model.EndDate;
            c.PageUrl = model.PageUrl;
            c.RateCodeId = model.RateCodeId;
            c.FileUploadId = model.FileUploadId;
            c.SearchResultIconFileUploadId = model.SearchResultIconFileUploadId;
            c.IsArchived = false;

            _db.Campaigns.Add(c);
            _db.SaveChanges();

            model.Id = c.Id;

            return model;
        }

        public List<CampaignModel> Get(CampaignFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);

            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public CampaignModel GetById(int id) {
            CampaignModel c = getAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (c == null)
                throw new Exception(CampaignNotFound);
            return c;
        }

        public CampaignModel GetByUrl(string url) {
            CampaignModel c = getAsIQueryable().SingleOrDefault(x => x.PageUrl == url);
            if (c == null)
                throw new Exception(CampaignNotFound);
            return c;
        }

        public int GetCount(CampaignFilterModel filter) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public CampaignModel Update(CampaignModel model) {
            Campaign c = _db.Campaigns.SingleOrDefault(x => x.Id == model.Id);
            if (c == null)
                throw new Exception(CampaignNotFound);

            c.Title = model.Title;
            c.Description = model.Description;
            c.RateCodeId = model.RateCodeId;
            c.StartDate = model.StartDate;
            c.EndDate = model.EndDate;
            c.PageUrl = model.PageUrl;
            c.FileUploadId = model.FileUploadId;
            c.SearchResultIconFileUploadId = model.SearchResultIconFileUploadId;
            c.IsArchived = model.IsArchived;

            _db.SaveChanges();

            return model;
        }


        private IQueryable<CampaignModel> applyFilter(IQueryable<CampaignModel> list, CampaignFilterModel filter) {
            if (filter != null) {
                if (!string.IsNullOrEmpty(filter.Title))
                    list = list.Where(x => x.Title.Contains(filter.Title));
                if (filter.RateCodeId.HasValue)
                    list = list.Where(x => x.RateCodeId == filter.RateCodeId);
                if (filter.StartDate.HasValue)
                    list = list.Where(x => x.StartDate == filter.StartDate);
                if (filter.EndDate.HasValue)
                    list = list.Where(x => x.EndDate == filter.EndDate);
                if (filter.IsArchived.HasValue)
                    list = list.Where(x => x.IsArchived == filter.IsArchived);
            }

            return list;
        }
        private IQueryable<CampaignModel> getAsIQueryable() {
            return _db.Campaigns.Select(x => new CampaignModel {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PageUrl = x.PageUrl,
                RateCodeId = x.RateCodeId,
                RateCode = new RateCodeModel {
                    Id = x.RateCode.Id,
                    Title = x.RateCode.Title,
                    AvailableToPublic = x.RateCode.AvailableToPublic,
                    AvailableToCorporate = x.RateCode.AvailableToCorporate,
                    AvailableToLoyalty = x.RateCode.AvailableToLoyalty
                },
                IsArchived = x.IsArchived,
                FileUploadId = x.FileUploadId,
                SearchResultIconFileUploadId = x.SearchResultIconFileUploadId,
                CampaignImage = x.FileUploadId.HasValue ? (new FileUploadModel {
                    Id = x.FileUpload.Id,
                    Title = x.FileUpload.Title,
                    FileExtension = x.FileUpload.FileExtension,
                    DateUploaded = x.FileUpload.DateUploaded
                }) : null,
                SearchResultIconImage = x.SearchResultIconFileUploadId.HasValue ? (new FileUploadModel {
                    Id = x.FileUpload1.Id,
                    Title = x.FileUpload1.Title,
                    FileExtension = x.FileUpload1.FileExtension,
                    DateUploaded = x.FileUpload1.DateUploaded
                }) : null
            });

        }
    }
}
