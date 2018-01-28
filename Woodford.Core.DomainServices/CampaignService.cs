using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DomainModel.Common;
    using Woodford.Core.DomainModel.Enums;
    using Woodford.Core.DomainModel.Models;
    using Woodford.Core.Interfaces;
    public class CampaignService : ICampaignService {

        private readonly ICampaignRepository _repo;
        private readonly IFileUploadService _fileUploads;
        private readonly IPageContentService _pageContentService;

        public CampaignService(ICampaignRepository repo, IFileUploadService fileUploads, IPageContentService pageContentService) {
            _repo = repo;
            _fileUploads = fileUploads;
            _pageContentService = pageContentService;
        }
        public CampaignModel Create(CampaignModel model) {
            if (model.CampaignImage != null) {
                FileUploadModel f = _fileUploads.Create(model.CampaignImage);
                model.FileUploadId = f.Id;
            }
            if (model.SearchResultIconImage != null) {
                FileUploadModel f = _fileUploads.Create(model.SearchResultIconImage, false);
                model.SearchResultIconFileUploadId = f.Id;
            }
            CampaignModel c = _repo.Create(model);
            model.PageContent.CampaignId = c.Id;
            c.PageContent = _pageContentService.Create(model.PageContent);
            return c;
        }

        public ListOf<CampaignModel> Get(CampaignFilterModel filter, ListPaginationModel pagination) {            
            ListOf<CampaignModel> res = new ListOf<CampaignModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }
            return res;
        }

        public CampaignModel GetById(int id, bool includePageContent = false) {
            CampaignModel c = _repo.GetById(id);
            if (includePageContent) {
                c.PageContent = _pageContentService.GetByForeignKey(c.Id, PageContentForeignKey.CampaignId);
            }
            return c;
        }

        public CampaignModel GetByUrl(string url, bool includePageContent = false) {
            CampaignModel c = _repo.GetByUrl(url);
            if (includePageContent) {
                c.PageContent = _pageContentService.GetByForeignKey(c.Id, PageContentForeignKey.CampaignId);
            }
            return c;
        }

        public CampaignModel Update(CampaignModel model) {

            if (model.CampaignImage != null) {
                if (model.CampaignImage.FileContents != null) {
                    if (model.FileUploadId.HasValue) {
                        model.CampaignImage.Id = model.FileUploadId.Value;
                        model.FileUploadId = _fileUploads.Update(model.CampaignImage).Id;
                    } else {
                        FileUploadModel f = _fileUploads.Create(model.CampaignImage);
                        model.FileUploadId = f.Id;
                    }
                }
            }

            if (model.SearchResultIconImage != null) {
                if (model.SearchResultIconImage.FileContents != null) {
                    if (model.SearchResultIconFileUploadId.HasValue) {
                        model.SearchResultIconImage.Id = model.SearchResultIconFileUploadId.Value;
                        model.SearchResultIconFileUploadId = _fileUploads.Update(model.SearchResultIconImage).Id;
                    }
                    else {
                        FileUploadModel f = _fileUploads.Create(model.SearchResultIconImage, false);
                        model.SearchResultIconFileUploadId = f.Id;
                    }
                }
            }

            model.PageContent = _pageContentService.Update(model.PageContent);

            return _repo.Update(model);
        }
    }
}
