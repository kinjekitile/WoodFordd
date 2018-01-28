using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Services;

namespace Woodford.Core.ApplicationServices.Queries {
    public class EmailSignatureGetRenderedSignatureQuery : IQuery<byte[]> {
        public int Id { get; set; }
    }

    public class EmailSignatureGetRenderedSignatureQueryHandler : IQueryHandler<EmailSignatureGetRenderedSignatureQuery, byte[]> {

        private readonly IEmailSignatureService _emailSignatureService;
        private readonly IEmailSignatureCampaignService _campaignService;

        public EmailSignatureGetRenderedSignatureQueryHandler(IEmailSignatureService emailSignatureService, IEmailSignatureCampaignService campaignService) {
            _emailSignatureService = emailSignatureService;
            _campaignService = campaignService;
        }

        public byte[] Process(EmailSignatureGetRenderedSignatureQuery query) {
            var emailSignatureDetails = _emailSignatureService.GetById(query.Id);

            //Get current live campaign


            throw new NotImplementedException();
        }
    }
}
