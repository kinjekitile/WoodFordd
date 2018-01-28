using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
	public class DbConnectionConfig : IDbConnectionConfig {
		public string GetConnectionString() {
			return ConfigurationManager.ConnectionStrings["WoodfordEntities"].ConnectionString;
		}
		public string GetConnectionStringName() {
			return "WoodfordEntities";
		}


		public string GetMembershipConnectionString() {
			return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			
		}

		public string GetMembershipConnectionStringName() {
			return "DefaultConnection";
		}
	}
}
