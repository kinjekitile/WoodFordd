using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VoucherGetByIdQuery : IQuery<VoucherModel> {
        public int Id { get; set; }
    }

    public class VoucherGetByIdQueryHandler : IQueryHandler<VoucherGetByIdQuery, VoucherModel> {
        private readonly IVoucherService _voucherService;
        public VoucherGetByIdQueryHandler(IVoucherService voucherService) {
            _voucherService = voucherService;
        }

        public VoucherModel Process(VoucherGetByIdQuery query) {
            return _voucherService.GetById(query.Id);
        }
    }
}
