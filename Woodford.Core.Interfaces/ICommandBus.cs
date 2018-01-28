using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.Interfaces {
	public interface ICommandBus {
		void Submit<TCommand>(TCommand command) where TCommand : ICommand;
	}
}
