using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class HerospaceRepository : RepositoryBase, IHerospaceRepository {

        private const string ItemNotFound = "Herospace item could not be found";

        public HerospaceRepository(IDbConnectionConfig connection) : base(connection) { }

        public HerospaceItemModel Create(HerospaceItemModel model) {
            HerospaceItem i = new HerospaceItem();
            i.Title = model.Title;
            i.LinkUrl = model.LinkUrl;
            i.FileUploadId = model.FileUploadId;
            i.SortOrder = model.SortOrder;
            i.IsArchived = model.IsArchived;
            _db.HerospaceItems.Add(i);
            _db.SaveChanges();
            model.Id = i.Id;
            return model;
        }

        public List<HerospaceItemModel> Get(HerospaceItemFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.IsArchived.HasValue) list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
            }
            return list.ToList();
        }

        public HerospaceItemModel GetById(int id) {
            HerospaceItemModel i = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (i == null) throw new Exception(ItemNotFound);
            return i;
        }

        public HerospaceItemModel Update(HerospaceItemModel model) {
            HerospaceItem i = _db.HerospaceItems.Where(x => x.Id == model.Id).SingleOrDefault();
            if (i == null) throw new Exception(ItemNotFound);

            i.Title = model.Title;
            i.LinkUrl = model.LinkUrl;
            i.FileUploadId = model.FileUploadId;
            i.IsArchived = model.IsArchived;
            i.SortOrder = model.SortOrder;
            _db.SaveChanges();

            return model;
        }

        private IQueryable<HerospaceItemModel> getAsIQueryable() {
            return _db.HerospaceItems.Select(x => new HerospaceItemModel {
                Id = x.Id,
                Title = x.Title,
                IsArchived = x.IsArchived,
                LinkUrl = x.LinkUrl,
                FileUploadId = x.FileUploadId,
                SortOrder = x.SortOrder,
                HerospaceImage = new FileUploadModel {
                    Id = x.FileUpload.Id,
                    Title = x.FileUpload.Title,
                    FileExtension = x.FileUpload.FileExtension,
                    DateUploaded = x.FileUpload.DateUploaded
                }
            });
        }
    }
}
