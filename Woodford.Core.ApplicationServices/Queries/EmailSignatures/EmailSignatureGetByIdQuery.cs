using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Services;

namespace Woodford.Core.ApplicationServices.Queries {
    public class EmailSignatureGetByIdQuery : IQuery<EmailSignatureModel> {
        public int Id { get; set; }
        
    }

    public class EmailSignatureGetByIdQueryHandler : IQueryHandler<EmailSignatureGetByIdQuery, EmailSignatureModel> {
        private readonly IEmailSignatureService _emailSignatureService;
        public EmailSignatureGetByIdQueryHandler(IEmailSignatureService emailSignatureService) {
            _emailSignatureService = emailSignatureService;
        }

        public EmailSignatureModel Process(EmailSignatureGetByIdQuery query) {
            return _emailSignatureService.GetById(query.Id);
        }
    }
}
