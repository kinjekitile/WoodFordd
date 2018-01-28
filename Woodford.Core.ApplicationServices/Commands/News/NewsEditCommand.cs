using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NewsEditCommand : ICommand {
        public NewsModel Model { get; set; }               
    }

    public class NewsEditCommandHandler : ICommandHandler<NewsEditCommand> {
        private readonly INewsService _newsService;
        public NewsEditCommandHandler(INewsService newsService) {
            _newsService = newsService;
        }

        public void Handle(NewsEditCommand command) {
            command.Model = _newsService.Update(command.Model);
        }
    }
}
