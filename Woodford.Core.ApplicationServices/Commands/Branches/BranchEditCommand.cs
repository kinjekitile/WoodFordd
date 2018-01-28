using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchEditCommand : ICommand {
        public BranchModel Model { get; set; }               
    }

    public class BranchEditCommandHandler : ICommandHandler<BranchEditCommand> {
        private readonly IBranchService _branchService;
        private readonly IPageContentService _pageContentService;
        public BranchEditCommandHandler(IBranchService branchService, IPageContentService pageContentService) {
            _branchService = branchService;
            _pageContentService = pageContentService;
        }

        public void Handle(BranchEditCommand command) {
            _branchService.Update(command.Model);
            _pageContentService.Update(command.Model.PageContent);
        }
    }
}
