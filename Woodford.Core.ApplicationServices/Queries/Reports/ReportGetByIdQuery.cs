using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class ReportGetByIdQuery : IQuery<ReportModel> {
        public int Id { get; set; }
        
    }

    public class ReportGetByIdQueryHandler : IQueryHandler<ReportGetByIdQuery, ReportModel> {
        private readonly IReportService _reportService;
        public ReportGetByIdQueryHandler(IReportService reportService) {
            _reportService = reportService;
        }

        public ReportModel Process(ReportGetByIdQuery query) {
            return _reportService.GetById(query.Id);
        }
    }
}
