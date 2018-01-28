using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.Interfaces {
	public interface IQueryProcessor {
		TResult Process<TResult>(IQuery<TResult> query);
	}
}
