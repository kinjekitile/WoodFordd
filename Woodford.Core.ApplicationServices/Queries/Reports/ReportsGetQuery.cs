using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class ReportsGetQuery : IQuery<ListOf<ReportModel>> {
        public ReportFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class ReportsGetQueryHandler : IQueryHandler<ReportsGetQuery, ListOf<ReportModel>> {
        private readonly IReportService _branchService;
        public ReportsGetQueryHandler(IReportService branchService) {
            _branchService = branchService;
        }

        public ListOf<ReportModel> Process(ReportsGetQuery query) {
            return _branchService.Get(query.Filter, query.Pagination);
        }
    }
}
