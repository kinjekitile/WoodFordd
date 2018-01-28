using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VouchersGetQuery : IQuery<ListOf<VoucherModel>> {
        public VoucherFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class VouchersGetQueryHandler : IQueryHandler<VouchersGetQuery, ListOf<VoucherModel>> {
        private readonly IVoucherService _voucherService;
        public VouchersGetQueryHandler(IVoucherService voucherService) {
            _voucherService = voucherService;
        }

        public ListOf<VoucherModel> Process(VouchersGetQuery query) {
            return _voucherService.Get(query.Filter, query.Pagination);
        }
    }
}
