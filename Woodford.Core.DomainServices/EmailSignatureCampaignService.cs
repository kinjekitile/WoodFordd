using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces.Services;
using Woodford.Core.Interfaces.Repositories;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class EmailSignatureCampaignService : IEmailSignatureCampaignService {

        private readonly IEmailSignatureCampaignRepository _repo;
        private readonly IFileUploadService _fileUploads;

        public EmailSignatureCampaignService(IEmailSignatureCampaignRepository repo, IFileUploadService fileUploads) {
            _repo = repo;
            _fileUploads = fileUploads;
        }
        public EmailSignatureCampaignModel Create(EmailSignatureCampaignModel model) {

            if (model.MainContentFile != null) {
                FileUploadModel f = _fileUploads.Create(model.MainContentFile, resize: false);
                model.MainContentFileUploadId = f.Id;
            }

            if (model.MainContentNarrowFile != null) {
                FileUploadModel f = _fileUploads.Create(model.MainContentNarrowFile, resize: false);
                model.MainContentNarrowFileUploadId = f.Id;
            }

            if (model.FooterContentFile != null) {
                FileUploadModel f = _fileUploads.Create(model.FooterContentFile, resize: false);
                model.FooterContentFileUploadId = f.Id;
            }

            if (model.FooterContentNarrowFile != null) {
                FileUploadModel f = _fileUploads.Create(model.FooterContentNarrowFile, resize: false);
                model.FooterContentNarrowFileUploadId = f.Id;
            }

            if (model.SidePanelContentFile != null) {
                FileUploadModel f = _fileUploads.Create(model.SidePanelContentFile, resize: false);
                model.SidePanelContentFileUploadId = f.Id;
            }

            if (model.UnderlineContentFile != null) {
                FileUploadModel f = _fileUploads.Create(model.UnderlineContentFile, resize: false);
                model.UnderlineContentFileUploadId = f.Id;
            }

            return _repo.Create(model);
        }

        public ListOf<EmailSignatureCampaignModel> Get(EmailSignatureCampaignFilterModel filter, ListPaginationModel pagination) {

            ListOf<EmailSignatureCampaignModel> res = new ListOf<EmailSignatureCampaignModel>();

            res.Pagination = pagination;
            res.Items = _repo.Get(filter, res.Pagination);
            if (pagination != null) {
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }

            return res;
        }

        public EmailSignatureCampaignModel GetById(int id) {
            return _repo.GetById(id);
        }

        public EmailSignatureCampaignModel Update(EmailSignatureCampaignModel model) {
            if (model.MainContentFile != null) {
                if (model.MainContentFile.FileContents != null) {
                    model.MainContentFile.Id = model.MainContentFileUploadId;
                    model.MainContentFileUploadId = _fileUploads.Update(model.MainContentFile, resize: false).Id;
                }
            }

            if (model.MainContentNarrowFile != null) {
                if (model.MainContentNarrowFile.FileContents != null) {
                    model.MainContentNarrowFile.Id = model.MainContentNarrowFileUploadId;
                    model.MainContentNarrowFileUploadId = _fileUploads.Update(model.MainContentNarrowFile, resize: false).Id;
                }
            }

            if (model.FooterContentFile != null) {
                if (model.FooterContentFile.FileContents != null) {
                    model.FooterContentFile.Id = model.FooterContentFileUploadId;
                    model.FooterContentFileUploadId = _fileUploads.Update(model.FooterContentFile, resize: false).Id;
                }
            }

            if (model.FooterContentNarrowFile != null) {
                if (model.FooterContentNarrowFile.FileContents != null) {
                    model.FooterContentNarrowFile.Id = model.FooterContentNarrowFileUploadId;
                    model.FooterContentNarrowFileUploadId = _fileUploads.Update(model.FooterContentNarrowFile, resize: false).Id;
                }
            }

            if (model.SidePanelContentFile != null) {
                if (model.SidePanelContentFile.FileContents != null) {

                    model.SidePanelContentFile.Id = model.SidePanelContentFileUploadId;
                    model.SidePanelContentFileUploadId = _fileUploads.Update(model.SidePanelContentFile, resize: false).Id;


                }
            }

            if (model.UnderlineContentFile != null) {
                if (model.UnderlineContentFile.FileContents != null) {

                    model.UnderlineContentFile.Id = model.UnderlineContentFileUploadId;
                    model.UnderlineContentFileUploadId = _fileUploads.Update(model.UnderlineContentFile, resize: false).Id;


                }
            }

            return _repo.Update(model);
        }
    }
}
