using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NewsCategoryEditCommand : ICommand {
        public NewsCategoryModel Model { get; set; }               
    }

    public class NewsCategoryEditCommandHandler : ICommandHandler<NewsCategoryEditCommand> {
        private readonly INewsCategoryService _newsCategoryService;
        public NewsCategoryEditCommandHandler(INewsCategoryService newsCategoryService) {
            _newsCategoryService = newsCategoryService;
        }

        public void Handle(NewsCategoryEditCommand command) {
            command.Model = _newsCategoryService.Update(command.Model);
        }
    }
}
