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
    public class EmailSignatureCampaignGetQuery : IQuery<ListOf<EmailSignatureCampaignModel>> {
        public EmailSignatureCampaignFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class EmailSignatureCampaignGetQueryHandler : IQueryHandler<EmailSignatureCampaignGetQuery, ListOf<EmailSignatureCampaignModel>> {

        private readonly IEmailSignatureCampaignService _emailSignatureCampaignService;

        public EmailSignatureCampaignGetQueryHandler(IEmailSignatureCampaignService emailSignatureCampaignService) {
            _emailSignatureCampaignService = emailSignatureCampaignService;
        }

        public ListOf<EmailSignatureCampaignModel> Process(EmailSignatureCampaignGetQuery query) {
            return _emailSignatureCampaignService.Get(query.Filter, query.Pagination);
        }
    }
}
