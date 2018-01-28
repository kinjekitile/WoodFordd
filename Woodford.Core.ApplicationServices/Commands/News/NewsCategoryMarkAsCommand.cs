using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NewsCategoryMarkAsCommand : ICommand {
        public int Id { get; set; }
        public bool MarkAsArchived { get; set; }
        public NewsCategoryModel ModelOut { get; set; }               
    }

    public class NewsCategoryMarkAsCommandHandler : ICommandHandler<NewsCategoryMarkAsCommand> {
        private readonly INewsCategoryService _newsCategoryService;
        public NewsCategoryMarkAsCommandHandler(INewsCategoryService newsCategoryService) {
            _newsCategoryService = newsCategoryService;
        }

        public void Handle(NewsCategoryMarkAsCommand command) {
            command.ModelOut = _newsCategoryService.MarkAs(command.Id, command.MarkAsArchived);
        }
    }
}
