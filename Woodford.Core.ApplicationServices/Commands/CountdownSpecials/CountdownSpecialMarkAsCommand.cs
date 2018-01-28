using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class CountdownSpecialMarkAsCommand : ICommand {
        public int Id { get; set; }        
        public bool MarkAs { get; set; }
        public CountdownSpecialModel ModelOut { get; set; }
    }

    public class CountdownSpecialMarkAsCommandHandler : ICommandHandler<CountdownSpecialMarkAsCommand> {
        private readonly ICountdownSpecialService _countdownSpecialService;
        public CountdownSpecialMarkAsCommandHandler(ICountdownSpecialService countdownSpecialService) {
            _countdownSpecialService = countdownSpecialService;            
        }

        public void Handle(CountdownSpecialMarkAsCommand command) {
            command.ModelOut = _countdownSpecialService.MarkAs(command.Id, command.MarkAs);
        }
    }
}
