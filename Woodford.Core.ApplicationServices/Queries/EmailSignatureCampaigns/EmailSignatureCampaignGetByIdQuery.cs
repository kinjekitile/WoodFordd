using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Services;

namespace Woodford.Core.ApplicationServices.Queries {
    public class EmailSignatureCampaignGetByIdQuery : IQuery<EmailSignatureCampaignModel> {
        public int Id { get; set; }
        
    }

    public class EmailSignatureCampaignGetByIdQueryHandler : IQueryHandler<EmailSignatureCampaignGetByIdQuery, EmailSignatureCampaignModel> {
        private readonly IEmailSignatureCampaignService _emailSignatureCampaignService;
        public EmailSignatureCampaignGetByIdQueryHandler(IEmailSignatureCampaignService emailSignatureCampaignService) {
            _emailSignatureCampaignService = emailSignatureCampaignService;
        }

        public EmailSignatureCampaignModel Process(EmailSignatureCampaignGetByIdQuery query) {
            return _emailSignatureCampaignService.GetById(query.Id);
        }
    }
}
