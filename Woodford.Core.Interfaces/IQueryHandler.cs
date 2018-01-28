using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.Interfaces {
	public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> {
		TResult Process(TQuery query);
	}
}
