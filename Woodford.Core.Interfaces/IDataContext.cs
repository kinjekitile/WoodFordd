using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.Interfaces {
	public interface IDataContext {
		void AddObject(string entitySetName, object entity);
		//void Attach(IEntityWithKey entity);
		void Detach(object entity);
		void DeleteObject(object entity);
		int SaveChanges();
	}
}
