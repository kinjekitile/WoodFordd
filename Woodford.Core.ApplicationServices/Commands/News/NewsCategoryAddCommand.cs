using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NewsCategoryAddCommand : ICommand {
        public NewsCategoryModel Model { get; set; }               
    }

    public class NewsCategoryAddCommandHandler : ICommandHandler<NewsCategoryAddCommand> {
        private readonly INewsCategoryService _newsCategoryService;
        public NewsCategoryAddCommandHandler(INewsCategoryService newsCategoryService) {
            _newsCategoryService = newsCategoryService;
        }

        public void Handle(NewsCategoryAddCommand command) {
            command.Model = _newsCategoryService.Create(command.Model);
        }
    }
}
