using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
	public abstract class RepositoryBase {
		internal bool _isDbCreated = false;
		internal Woodford2015Entities _db;
        public RepositoryBase(IDbConnectionConfig connection)
        {
            _db = new Woodford2015Entities(connection);
            _isDbCreated = true;
        }

        public RepositoryBase()
        {
            _db = new Woodford2015Entities();
            _isDbCreated = true;
        }

  //      public virtual void Dispose() {
		//	if (_isDbCreated) {
		//		_db.Dispose();
		//	}
		//}
	}
}
