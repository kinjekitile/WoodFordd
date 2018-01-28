using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces.Repositories {
    public interface IUserActivityRepository {
        ListOf<UserActivityModel> Get(UserActivityFilterModel filter, ListPaginationModel pagination);
    }
}
