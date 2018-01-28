using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Providers;

namespace Woodford.Core.ApplicationServices.CommandDecorators {
    public class ExceptionLoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> {
		private readonly ICommandHandler<TCommand> _decorated;
		private ILoggingProvider _logging;

		public ExceptionLoggingCommandHandlerDecorator(ICommandHandler<TCommand> decorated, ILoggingProvider logging) {
			_decorated = decorated;
			_logging = logging;
		}

		public void Handle(TCommand command) {
			try {
				this._decorated.Handle(command);
			} catch (Exception ex) {                
				_logging.LogException(ex);
				throw;
			}
			
		}
	}
}
