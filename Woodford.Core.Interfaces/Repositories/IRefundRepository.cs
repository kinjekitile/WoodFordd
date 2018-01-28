using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRefundRepository {
        RefundModel Create(RefundModel model);
        RefundModel Update(RefundModel model);
        RefundModel GetById(int id);
        List<RefundModel> Get(RefundFilterModel filter, ListPaginationModel pagination);
        int GetCount(RefundFilterModel filter);
    }
}
