using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Infrastructure.Data {
    public class ReportRepository : RepositoryBase, IReportRepository {
        public ReportRepository(IDbConnectionConfig connection) : base(connection) {

        }

        public ReportModel Create(ReportModel model) {
            Report r = new Report();
            r.Title = model.Title;
            r.ReportType = (int)model.ReportType;

            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            if (model.ReportType == ReportType.Reservation) {
                XmlSerializer serializer = new XmlSerializer(typeof(ReservationFilterModel));
                serializer.Serialize(writer, model.ReservationFilter);
            }
            if (model.ReportType == ReportType.User) {
                XmlSerializer serializer = new XmlSerializer(typeof(UserFilterModel));
                serializer.Serialize(writer, model.UserFilter);
            }
            string result = writer.ToString();
            r.ReportFilter = result;
            //r.ReportFilter = model.ReportFilter;
            r.StartDate = model.StartDate;
            r.UseCurrentDateAsStartDate = model.UseCurrentDateAsStartDate;
            r.DateUnitsToAdd = model.DateUnitsToAdd;
            r.DateUnitType = (int?)model.DateUnitType;
            r.CreatedDate = DateTime.Now;

            _db.Reports.Add(r);
            _db.SaveChanges();

            model.Id = r.Id;
            model.CreatedDate = r.CreatedDate;
            return model;
        }

        public void Delete(int id) {
            Report r = _db.Reports.SingleOrDefault(x => x.Id == id);
            if (r == null) {
                throw new Exception("Report not found");
            }
            _db.Reports.Remove(r);
            _db.SaveChanges();
        }

        public List<ReportModel> Get(ReportFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsQueryable();
            list = applyFilter(list, filter);
            List<ReportModel> results = new List<ReportModel>();

            if (pagination == null) {
                results = list.ToList();
            } else {
                results =  list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }

            foreach (var res in results) {

                if (res.ReportType == ReportType.Reservation) {
                    XmlSerializer serializer = new XmlSerializer(typeof(ReservationFilterModel));
                    using (TextReader reader = new StringReader(res.ReportFilter)) {
                        ReservationFilterModel itemFilter = (ReservationFilterModel)serializer.Deserialize(reader);
                        res.ReservationFilter = itemFilter;
                    }
                    
                }
                if (res.ReportType == ReportType.User) {
                    XmlSerializer serializer = new XmlSerializer(typeof(UserFilterModel));
                    using (TextReader reader = new StringReader(res.ReportFilter)) {
                        UserFilterModel itemFilter = (UserFilterModel)serializer.Deserialize(reader);
                        res.UserFilter = itemFilter;
                    }

                }
            }

            return results;
        }

        public ReportModel GetById(int id) {
            ReportModel r = GetAsQueryable().SingleOrDefault(x => x.Id == id);
           
            if (r == null) {
                throw new Exception("Report not found");
            }
            XmlSerializer serializer = new XmlSerializer(typeof(ReservationFilterModel));
            using (TextReader reader = new StringReader(r.ReportFilter)) {
                ReservationFilterModel filter = (ReservationFilterModel)serializer.Deserialize(reader);
                r.ReservationFilter = filter;
            }
            return r;
        }

        public int GetCount(ReportFilterModel filter) {
            var list = GetAsQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public ReportModel Update(ReportModel model) {
            Report r = _db.Reports.SingleOrDefault(x => x.Id == model.Id);
            if (r == null) {
                throw new Exception("Report not found");
            }
            r.Title = model.Title;
            r.ReportType = (int)model.ReportType;
            r.ReportFilter = model.ReportFilter;
            r.StartDate = model.StartDate;
            r.UseCurrentDateAsStartDate = model.UseCurrentDateAsStartDate;
            r.DateUnitsToAdd = model.DateUnitsToAdd;
            r.DateUnitType = (int)model.DateUnitType;

            _db.SaveChanges();

            return model;
        }

        private IQueryable<ReportModel> applyFilter(IQueryable<ReportModel> list, ReportFilterModel filter) {
            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.ReportType.HasValue)
                    list = list.Where(x => x.ReportType == filter.ReportType.Value);
                if (!string.IsNullOrEmpty(filter.Title))
                    list = list.Where(x => x.Title.Contains(filter.Title));
                if (filter.CreatedDate.HasValue)
                    list = list.Where(x => x.CreatedDate == filter.CreatedDate.Value);
            }

            return list;
        }

        private IQueryable<ReportModel> GetAsQueryable() {
            return _db.Reports.Select(x => new ReportModel {
                Id = x.Id,
                Title = x.Title,
                ReportType = (ReportType)x.ReportType,
                ReportFilter = x.ReportFilter,
                CreatedDate = x.CreatedDate,
                StartDate = x.StartDate,
                UseCurrentDateAsStartDate = x.UseCurrentDateAsStartDate,
                DateUnitsToAdd = x.DateUnitsToAdd,
                DateUnitType = (ReportDateUnitType)x.DateUnitType
            });
            
        }

    }
}
