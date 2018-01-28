using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IReportService {
        ReportModel Create(ReportModel model);
        ReportModel Update(ReportModel model);
        ReportModel GetById(int id);
        ListOf<ReportModel> Get(ReportFilterModel filter, ListPaginationModel pagination);
        int GetCount(ReportFilterModel filter);
        void Delete(int id);
    }
}
