using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Code {
	public class DefaultCommandBus : ICommandBus {

		public void Submit<TCommand>(TCommand command) where TCommand : ICommand {
            var handler = MvcApplication.Container.GetInstance<ICommandHandler<TCommand>>();
            if (!((handler != null) && handler is ICommandHandler<TCommand>))
            {
                throw new Exception("Command Handler not found");
            }
            handler.Handle(command);
        }
	}
}