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
    public class EmailSignatureGetQuery : IQuery<ListOf<EmailSignatureModel>> {
        public EmailSignatureFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class EmailSignatureGetQueryHandler : IQueryHandler<EmailSignatureGetQuery, ListOf<EmailSignatureModel>> {

        private readonly IEmailSignatureService _emailSignatureService;

        public EmailSignatureGetQueryHandler(IEmailSignatureService emailSignatureService) {
            _emailSignatureService = emailSignatureService;
        }

        public ListOf<EmailSignatureModel> Process(EmailSignatureGetQuery query) {
            return _emailSignatureService.Get(query.Filter, query.Pagination);
        }
    }
}
