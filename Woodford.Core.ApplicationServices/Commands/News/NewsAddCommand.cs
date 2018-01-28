using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NewsAddCommand : ICommand {
        public NewsModel Model { get; set; }               
    }

    public class NewsAddCommandHandler : ICommandHandler<NewsAddCommand> {
        private readonly INewsService _newsService;
        public NewsAddCommandHandler(INewsService newsService) {
            _newsService = newsService;
        }

        public void Handle(NewsAddCommand command) {
            command.Model = _newsService.Create(command.Model);
        }
    }
}
