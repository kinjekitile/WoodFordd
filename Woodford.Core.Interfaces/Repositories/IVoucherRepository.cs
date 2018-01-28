using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IVoucherRepository {
        VoucherModel Create(VoucherModel model);
        VoucherModel Update(VoucherModel model);
        VoucherModel GetById(int id);
        VoucherModel GetByVoucherNumber(string voucherNumber);
        List<VoucherModel> Get(VoucherFilterModel filter, ListPaginationModel pagination);
        int GetCount(VoucherFilterModel filter);
    }
}
