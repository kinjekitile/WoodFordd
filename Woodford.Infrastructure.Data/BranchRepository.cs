using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class BranchRepository : RepositoryBase, IBranchRepository {

        private const string BranchNotFound = "Branch could not be found";

        public BranchRepository(IDbConnectionConfig connection) : base(connection) { }

        public BranchModel Create(BranchModel model) {
            Branch b = new Branch();
            b.Title = model.Title;
            b.FileUploadId = model.FileUploadId;
            b.DateAdded = DateTime.Now;
            b.IsAirport = model.IsAirport;
            b.Email = model.Email;
            b.MobileNumber = model.MobileNumber;
            b.TelephoneNumber = model.TelephoneNumber;
            b.FaxNumber = model.FaxNumber;
            b.MapEmbed = model.MapEmbed;
            b.AfterHoursTelephoneNumber = model.AfterHoursTelephoneNumber;
            b.PageUrl = model.PageUrl;
            b.IsArchived = false;
            b.BranchSubTitle = model.BranchSubTitle;
            b.BranchAddress = model.BranchAddress;
            b.BranchShortDescription = model.BranchShortDescription;
            b.BranchContactPageTitle = model.BranchContactPageTitle;
            b.BranchContactMetaKeywords = model.BranchContactMetaKeywords;
            b.BranchContactMetaDescription = model.BranchContactMetaDescription;
            b.BranchContactPageContent = model.BranchContactPageContent;
            b.BranchRequestCallbackPageTitle = model.BranchRequestCallbackPageTitle;
            b.BranchRequestCallbackMetaKeywords = model.BranchRequestCallbackMetaKeywords;
            b.BranchRequestCallbackMetaDescription = model.BranchRequestCallbackMetaDescription;
            b.BranchRequestCallbackPageContent = model.BranchRequestCallbackPageContent;
            b.WeatherApiId = model.WeatherApiId;
            b.BookingLeadTimeDay = model.BookingLeadTimeDay;
            b.BookingLeadTimeNight = model.BookingLeadTimeNight;
            _db.Branches.Add(b);
            _db.SaveChanges();
            model.Id = b.Id;
            return model;
        }

        public BranchModel Update(BranchModel model) {

            Branch b = _db.Branches.Where(x => x.Id == model.Id).SingleOrDefault();
            if (b == null)
                throw new Exception(BranchNotFound);
            b.Title = model.Title;
            b.FileUploadId = model.FileUploadId;
            b.IsAirport = model.IsAirport;
            b.Email = model.Email;
            b.MobileNumber = model.MobileNumber;
            b.TelephoneNumber = model.TelephoneNumber;
            b.FaxNumber = model.FaxNumber;
            b.MapEmbed = model.MapEmbed;
            b.AfterHoursTelephoneNumber = model.AfterHoursTelephoneNumber;
            b.PageUrl = model.PageUrl;
            b.IsArchived = model.IsArchived;
            b.BranchSubTitle = model.BranchSubTitle;
            b.BranchAddress = model.BranchAddress;
            b.BranchShortDescription = model.BranchShortDescription;
            b.BranchContactPageTitle = model.BranchContactPageTitle;
            b.BranchContactMetaKeywords = model.BranchContactMetaKeywords;
            b.BranchContactMetaDescription = model.BranchContactMetaDescription;
            b.BranchContactPageContent = model.BranchContactPageContent;
            b.BranchRequestCallbackPageTitle = model.BranchRequestCallbackPageTitle;
            b.BranchRequestCallbackMetaKeywords = model.BranchRequestCallbackMetaKeywords;
            b.BranchRequestCallbackMetaDescription = model.BranchRequestCallbackMetaDescription;
            b.BranchRequestCallbackPageContent = model.BranchRequestCallbackPageContent;
            b.WeatherApiId = model.WeatherApiId;
            b.BookingLeadTimeDay = model.BookingLeadTimeDay;
            b.BookingLeadTimeNight = model.BookingLeadTimeNight;

            _db.SaveChanges();


            return model;
        }

        public List<BranchModel> Get(BranchFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public int GetCount(BranchFilterModel filter) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        private IQueryable<BranchModel> applyFilter(IQueryable<BranchModel> list, BranchFilterModel filter) {
            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.IsAirport.HasValue)
                    list = list.Where(x => x.IsAirport == filter.IsAirport.Value);
                if (filter.IsArchived.HasValue)
                    list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
                if (filter.Ids != null)
                    list = list.Where(x => filter.Ids.Contains(x.Id));
            }
            return list;
        }

        public BranchModel GetById(int id) {
            BranchModel b = GetAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (b == null)
                throw new Exception(BranchNotFound);
            return b;
        }

        private IQueryable<BranchModel> GetAsIQueryable() {
            return _db.Branches.Select(x => new BranchModel {
                Id = x.Id,
                Title = x.Title,
                FileUploadId = x.FileUploadId,
                DateAdded = x.DateAdded,
                IsAirport = x.IsAirport,
                Email = x.Email,
                MobileNumber = x.MobileNumber,
                TelephoneNumber = x.TelephoneNumber,
                FaxNumber = x.FaxNumber,
                MapEmbed = x.MapEmbed,
                AfterHoursTelephoneNumber = x.AfterHoursTelephoneNumber,
                IsArchived = x.IsArchived,
                PageUrl = x.PageUrl,
                BranchSubTitle = x.BranchSubTitle,
                BranchAddress = x.BranchAddress,
                BranchShortDescription = x.BranchShortDescription,
                BranchImage = x.FileUploadId.HasValue ? (new FileUploadModel {
                    Id = x.FileUpload.Id,
                    Title = x.FileUpload.Title,
                    FileExtension = x.FileUpload.FileExtension,
                    DateUploaded = x.FileUpload.DateUploaded
                }) : null,
                BranchContactPageTitle = x.BranchContactPageTitle,
                BranchContactMetaKeywords = x.BranchContactMetaKeywords,
                BranchContactMetaDescription = x.BranchContactMetaDescription,
                BranchContactPageContent = x.BranchContactPageContent,
                BranchRequestCallbackPageTitle = x.BranchRequestCallbackPageTitle,
                BranchRequestCallbackMetaKeywords = x.BranchRequestCallbackMetaKeywords,
                BranchRequestCallbackMetaDescription = x.BranchRequestCallbackMetaDescription,
                BranchRequestCallbackPageContent = x.BranchRequestCallbackPageContent,
                WeatherApiId = x.WeatherApiId,
                BookingLeadTimeDay = x.BookingLeadTimeDay,
                BookingLeadTimeNight = x.BookingLeadTimeNight
            });
        }

        public BranchModel GetByUrl(string url) {
            BranchModel b = GetAsIQueryable().Where(x => x.PageUrl == url).SingleOrDefault();
            if (b == null)
                throw new Exception(BranchNotFound);
            return b;
        }
    }
}
