using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Services;

namespace Woodford.Core.ApplicationServices.Queries {
    public class EmailSignatureGetImageQuery : IQuery<byte[]> {
        public int Id { get; set; }

    }

    public class EmailSignatureGetImageQueryHandler : IQueryHandler<EmailSignatureGetImageQuery, byte[]> {
        private readonly IEmailSignatureService _emailSignatureService;
        private readonly IEmailSignatureCampaignService _emailSignatureCampaigService;
        public EmailSignatureGetImageQueryHandler(IEmailSignatureService emailSignatureService, IEmailSignatureCampaignService emailSignatureCampaigService) {
            _emailSignatureService = emailSignatureService;
            _emailSignatureCampaigService = emailSignatureCampaigService;
        }

        public byte[] Process(EmailSignatureGetImageQuery query) {
            var signature = _emailSignatureService.GetById(query.Id);
            var defaultCampaign = _emailSignatureCampaigService.Get(new EmailSignatureCampaignFilterModel { IsDefault = true }, null).Items.FirstOrDefault();
            var liveCampaign = _emailSignatureCampaigService.Get(new EmailSignatureCampaignFilterModel { IsLive = true }, null).Items.FirstOrDefault();

            if (liveCampaign != null) {
                return _emailSignatureService.GetEmailSignatureImageData(signature, liveCampaign);
            } else {
                if (defaultCampaign != null) {
                    return _emailSignatureService.GetEmailSignatureImageData(signature, defaultCampaign);
                }
                else {
                    return null;
                }
            }
            
        }
    }
}
