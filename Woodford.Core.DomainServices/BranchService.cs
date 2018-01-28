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
    public class BranchService : IBranchService {
        private readonly IBranchRepository _repo;
        private readonly IFileUploadService _fileUploads;
        private readonly IPageContentService _pageContentService;
        private readonly ISettingService _settingService;
        public BranchService(IBranchRepository repo, IFileUploadService fileUploads, IPageContentService pageContentService, ISettingService settingService) {
            _repo = repo;
            _fileUploads = fileUploads;
            _pageContentService = pageContentService;
            _settingService = settingService;
        }
        public BranchModel Create(BranchModel model) {
            if (model.BranchImage != null) {
                FileUploadModel f = _fileUploads.Create(model.BranchImage);
                model.FileUploadId = f.Id;
            }

            BranchModel b = _repo.Create(model);
            b.PageContent.BranchId = b.Id;
            b.PageContent = _pageContentService.Create(b.PageContent);

            return b;
        }

        public ListOf<BranchModel> Get(BranchFilterModel filter, ListPaginationModel pagination) {

            ListOf<BranchModel> res = new ListOf<BranchModel>();

            res.Pagination = pagination;
            res.Items = _repo.Get(filter, res.Pagination);
            if (pagination != null) {    
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }

            return res;
        }

        public BranchModel GetById(int id, bool includePageContent) {
            BranchModel b = _repo.GetById(id);
            if (includePageContent) {
                b.PageContent = _pageContentService.GetByForeignKey(b.Id, PageContentForeignKey.BranchId);
            }
            return b;
        }

        public BranchModel Update(BranchModel model) {
            if (model.BranchImage != null) {
                if (model.BranchImage.FileContents != null) {
                    if (model.FileUploadId.HasValue) {
                        model.BranchImage.Id = model.FileUploadId.Value;
                        model.FileUploadId = _fileUploads.Update(model.BranchImage).Id;                        
                    } else {
                        FileUploadModel f = _fileUploads.Create(model.BranchImage);
                        model.FileUploadId = f.Id;
                    }
                }
            }
            BranchModel b = _repo.Update(model);
            _pageContentService.Update(model.PageContent);

            return b;
        }

        public BranchModel GetByUrl(string url, bool includePageContent = false) {
            BranchModel b = _repo.GetByUrl(url);
            if (includePageContent) {
                b.PageContent = _pageContentService.GetByForeignKey(b.Id, PageContentForeignKey.BranchId);
            }
            return b;
        }

        public decimal GetTaxRateByBranchId(int id) {
            BranchModel b = _repo.GetById(id);
            decimal taxRate = 0m;
            if (b.IsAirport) {
                taxRate = Convert.ToDecimal(_settingService.Get(Setting.Tax_Rate_Airport).Value);
            } else {
                taxRate = Convert.ToDecimal(_settingService.Get(Setting.Tax_Rate_Non_Airport).Value);
            }
            return taxRate;
        }
    }
}
