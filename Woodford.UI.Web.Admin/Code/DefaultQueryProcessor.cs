using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Admin.Code {

	public class DefaultQueryProcessor : IQueryProcessor {
		public TResult Process<TResult>(IQuery<TResult> query) {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = MvcApplication.Container.GetInstance(handlerType);
            
            return handler.Process((dynamic)query);
            
		}
	}
}