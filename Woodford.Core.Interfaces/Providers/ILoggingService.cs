using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.Interfaces.Providers {
	public interface ILoggingProvider {
		void LogException(Exception ex);
	}
}
