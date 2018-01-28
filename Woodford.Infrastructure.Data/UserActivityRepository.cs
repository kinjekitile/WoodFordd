using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Repositories;

namespace Woodford.Infrastructure.Data {
    public class UserActivityRepository : RepositoryBase, IUserActivityRepository {
        public UserActivityRepository(IDbConnectionConfig connection) : base(connection) { }

        public ListOf<UserActivityModel> Get(UserActivityFilterModel filter, ListPaginationModel pagination) {
            throw new NotImplementedException();
        }

        private IQueryable<UserActivityModel> getAsQueryable() {
            //return _db.UserProfiles.Select(x => new UserActivityModel {
            //    Email = x.Email,
            //    FirstName = x.FirstName,
            //    LastName = x.LastName,
            //    LastBookedDate = x.Reservations.Where(y => y.Invoices.First().IsCompleted)
            //});
            throw new NotImplementedException();
        }
    }
}
