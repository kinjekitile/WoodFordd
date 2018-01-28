using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Repositories;

namespace Woodford.Infrastructure.Data {
    public class EmailSignatureCampaignRepository : RepositoryBase, IEmailSignatureCampaignRepository {

        public EmailSignatureCampaignRepository(IDbConnectionConfig connection) : base(connection) { }


        public EmailSignatureCampaignModel Create(EmailSignatureCampaignModel model) {

            if (model.IsDefault) {
                //if default then set all others to not default
                _db.EmailSignatureCampaigns.ToList().ForEach(x => x.IsDefault = false);
            }

            if (model.IsLive) {
                //All others not live
                _db.EmailSignatureCampaigns.ToList().ForEach(x => x.IsLive = false);
            }

            EmailSignatureCampaign c = new EmailSignatureCampaign();
            c.CampaignName = model.CampaignName;
            c.CreatedDate = DateTime.Now;
            c.IsDefault = model.IsDefault;
            c.IsLive = model.IsLive;
            c.MainContentFileUploadId = model.MainContentFileUploadId;
            c.MainContentNarrowFileUploadId = model.MainContentNarrowFileUploadId;
            c.FooterContentFileUploadId = model.FooterContentFileUploadId;
            c.FooterContentNarrowFileUploadId = model.FooterContentNarrowFileUploadId;
            c.SidePanelContentFileUploadId = model.SidePanelContentFileUploadId;
            c.UnderlineContentFileUploadId = model.UnderlineContentFileUploadId;
            _db.EmailSignatureCampaigns.Add(c);

            
            _db.SaveChanges();

            model.Id = c.Id;

            return model;
        }

        public List<EmailSignatureCampaignModel> Get(EmailSignatureCampaignFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            if (pagination == null) {
                return list.ToList();
            }
            else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public EmailSignatureCampaignModel GetById(int id) {
            EmailSignatureCampaignModel model = GetAsIQueryable().SingleOrDefault(x => x.Id == id);

            if (model == null) {
                throw new Exception("Email Signature Campaign could not be found");
            }

            return model;
        }

        public int GetCount(EmailSignatureCampaignFilterModel filter) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public EmailSignatureCampaignModel Update(EmailSignatureCampaignModel model) {
            EmailSignatureCampaign c = _db.EmailSignatureCampaigns.SingleOrDefault(x => x.Id == model.Id);

            if (c == null) {
                throw new Exception("Email Signature Campaing cannot be found");
            }

            c.CampaignName = model.CampaignName;
            c.CreatedDate = DateTime.Now;
            c.IsDefault = model.IsDefault;
            c.IsLive = model.IsLive;
            c.MainContentFileUploadId = model.MainContentFileUploadId;
            c.MainContentNarrowFileUploadId = model.MainContentNarrowFileUploadId;
            c.FooterContentFileUploadId = model.FooterContentFileUploadId;
            c.FooterContentNarrowFileUploadId = model.FooterContentNarrowFileUploadId;
            c.SidePanelContentFileUploadId = model.SidePanelContentFileUploadId;
            c.UnderlineContentFileUploadId = model.UnderlineContentFileUploadId;
            _db.SaveChanges();

            return model;

        }

        private IQueryable<EmailSignatureCampaignModel> applyFilter(IQueryable<EmailSignatureCampaignModel> list, EmailSignatureCampaignFilterModel filter) {

            if (filter != null) {
                if (filter.Id.HasValue) {
                    list = list.Where(x => x.Id == filter.Id.Value);
                }
                if (filter.IsDefault.HasValue) {
                    list = list.Where(x => x.IsDefault == filter.IsDefault);
                }
                if (filter.IsLive.HasValue) {
                    list = list.Where(x => x.IsLive == filter.IsLive.Value);
                }
            }

            return list;
        }

        private IQueryable<EmailSignatureCampaignModel> GetAsIQueryable() {
            return _db.EmailSignatureCampaigns.Select(x => new EmailSignatureCampaignModel {
                Id = x.Id,
                CampaignName = x.CampaignName,
                CreatedDate = x.CreatedDate,
                IsDefault = x.IsDefault,
                IsLive = x.IsLive,
                MainContentFileUploadId = x.MainContentFileUploadId,
                MainContentFile = new FileUploadModel {
                    Id = x.FileUploadMainContent.Id,
                    Title = x.FileUploadMainContent.Title,
                    FileExtension = x.FileUploadMainContent.FileExtension,
                    DateUploaded = x.FileUploadMainContent.DateUploaded
                },
                MainContentNarrowFileUploadId = x.MainContentNarrowFileUploadId,
                MainContentNarrowFile = new FileUploadModel {
                    Id = x.FileUploadMainContentNarrow.Id,
                    Title = x.FileUploadMainContentNarrow.Title,
                    FileExtension = x.FileUploadMainContentNarrow.FileExtension,
                    DateUploaded = x.FileUploadMainContentNarrow.DateUploaded
                },
                FooterContentFileUploadId = x.FooterContentFileUploadId,
                FooterContentFile = new FileUploadModel {
                    Id = x.FileUploadFooter.Id,
                    Title = x.FileUploadFooter.Title,
                    FileExtension = x.FileUploadFooter.FileExtension,
                    DateUploaded = x.FileUploadFooter.DateUploaded
                },
                FooterContentNarrowFileUploadId = x.FooterContentNarrowFileUploadId,
                FooterContentNarrowFile = new FileUploadModel {
                    Id = x.FileUploadFooterNarrow.Id,
                    Title = x.FileUploadFooterNarrow.Title,
                    FileExtension = x.FileUploadFooterNarrow.FileExtension,
                    DateUploaded = x.FileUploadFooterNarrow.DateUploaded
                },
                SidePanelContentFileUploadId = x.SidePanelContentFileUploadId,
                SidePanelContentFile = new FileUploadModel {
                    Id = x.FileUploadSidePanel.Id,
                    Title = x.FileUploadSidePanel.Title,
                    FileExtension = x.FileUploadSidePanel.FileExtension,
                    DateUploaded = x.FileUploadSidePanel.DateUploaded
                },
                UnderlineContentFileUploadId = x.UnderlineContentFileUploadId,
                UnderlineContentFile = new FileUploadModel {
                    Id = x.FileUploadUnderline.Id,
                    Title = x.FileUploadUnderline.Title,
                    FileExtension = x.FileUploadUnderline.FileExtension,
                    DateUploaded = x.FileUploadUnderline.DateUploaded
                }
            });
        }



    }
}
