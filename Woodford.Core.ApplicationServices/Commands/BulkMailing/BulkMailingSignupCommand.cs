using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Providers;

namespace Woodford.Core.ApplicationServices.Commands {
    public class BulkMailingSignupCommand : ICommand {
        public string Email { get; set; }
    }

    public class BulkMailingSignupCommandHandler : ICommandHandler<BulkMailingSignupCommand> {
        private readonly IBulkMailingService _bulkMailingService;
        public BulkMailingSignupCommandHandler(IBulkMailingService bulkMailingService) {
            _bulkMailingService = bulkMailingService;
        }
        public void Handle(BulkMailingSignupCommand command) {
            _bulkMailingService.Signup(command.Email);
        }
    }
}
