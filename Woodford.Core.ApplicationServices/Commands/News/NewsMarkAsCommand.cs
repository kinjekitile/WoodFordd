using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NewsMarkAsCommand : ICommand {
        public int Id { get; set; }
        public bool MarkAsArchived { get; set; }
        public NewsModel ModelOut { get; set; }               
    }

    public class NewsMarkAsCommandHandler : ICommandHandler<NewsMarkAsCommand> {
        private readonly INewsService _newsService;
        public NewsMarkAsCommandHandler(INewsService newsService) {
            _newsService = newsService;
        }

        public void Handle(NewsMarkAsCommand command) {
            command.ModelOut = _newsService.MarkAs(command.Id, command.MarkAsArchived);
        }
    }
}
