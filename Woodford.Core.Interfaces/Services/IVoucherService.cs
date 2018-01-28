using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IVoucherService {
        VoucherModel Create(VoucherModel model);
        VoucherModel Update(VoucherModel model);
        VoucherModel GetById(int id);
        VoucherModel GetByVoucherNumber(string voucherNumber);
        ListOf<VoucherModel> Get(VoucherFilterModel filter, ListPaginationModel pagination);        
        VoucherRedeemModel RedeemVoucher(string voucherNumber, string clientEmail, int reservationId);
        byte[] GeneratorVoucherImage(VoucherModel v, string voucherTemplatePath);
    }
}
