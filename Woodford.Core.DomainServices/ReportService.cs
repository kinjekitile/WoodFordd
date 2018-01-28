using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class ReportService : IReportService {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo) {
            _repo = repo;
        }
        public ReportModel Create(ReportModel model) {
            return _repo.Create(model);
        }

        public void Delete(int id) {
            _repo.Delete(id);
        }

        public ListOf<ReportModel> Get(ReportFilterModel filter, ListPaginationModel pagination) {

            ListOf<ReportModel> res = new ListOf<ReportModel>();

            res.Pagination = pagination;
            res.Items = _repo.Get(filter, res.Pagination);
            if (pagination != null) {
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }

            return res;
        }

        public ReportModel GetById(int id) {
            return _repo.GetById(id);
        }

        public int GetCount(ReportFilterModel filter) {
            return _repo.GetCount(filter);
        }

        public ReportModel Update(ReportModel model) {
            return _repo.Update(model);
        }
    }
}
