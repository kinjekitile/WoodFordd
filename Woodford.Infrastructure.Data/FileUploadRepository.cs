using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class FileUploadRepository : RepositoryBase, IFileUploadRepository {

        private const string FileNotFound = "File could not be found";

        public FileUploadRepository(IDbConnectionConfig connection) : base(connection) { }

        public FileUploadModel Create(FileUploadModel model) {
            FileUpload f = new FileUpload();
            f.Title = model.Title;
            f.FileContent = model.FileContents;
            f.FileExtension = model.FileExtension;
            f.DateUploaded = DateTime.Now;
            f.FileContext = model.FileContext;
            f.MimeType = model.MimeType;
            f.IsStandalone = model.IsStandalone;
            _db.FileUploads.Add(f);
            _db.SaveChanges();
            model.Id = f.Id;
            return model;
        }

        public List<FileUploadModel> Get(FileUploadFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.IsStandalone.HasValue) list = list.Where(x => x.IsStandalone == filter.IsStandalone.Value);
                if (!string.IsNullOrEmpty(filter.FileContext)) list = list.Where(x => x.FileContext == filter.FileContext);
            }            
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public int GetCount(FileUploadFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.IsStandalone.HasValue) list = list.Where(x => x.IsStandalone == filter.IsStandalone.Value);
                if (!string.IsNullOrEmpty(filter.FileContext)) list = list.Where(x => x.FileContext == filter.FileContext);
            }
            return list.Count();
        }

        public FileUploadModel GetById(int id, bool includeFileContents) {
            FileUploadModel f;
            if (includeFileContents) {
                f = getWithContentsAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            } else {
                f = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            }
            if (f == null)
                throw new Exception(FileNotFound);
            return f;
        }

        public FileUploadModel Update(FileUploadModel model) {

            FileUpload f = _db.FileUploads.Where(x => x.Id == model.Id).SingleOrDefault();
            if (f == null)
                throw new Exception(FileNotFound);
            f.Title = model.Title;
            if (model.FileContents != null) {
                f.FileContent = model.FileContents;
                f.FileExtension = model.FileExtension;
                f.DateUploaded = DateTime.Now;
                f.MimeType = model.MimeType;
            }            
            f.FileContext = model.FileContext;            
            _db.SaveChanges();

            return model;
        }

        private IQueryable<FileUploadModel> getAsIQueryable() {
            return _db.FileUploads.Select(x => new FileUploadModel {
                Id = x.Id,
                Title = x.Title,
                //FileContents = x.FileContent,
                FileExtension = x.FileExtension,
                DateUploaded = x.DateUploaded,
                FileContext = x.FileContext,
                MimeType = x.MimeType,
                IsStandalone = x.IsStandalone
            });
        }

        private IQueryable<FileUploadModel> getWithContentsAsIQueryable() {
            return _db.FileUploads.Select(x => new FileUploadModel {
                Id = x.Id,
                Title = x.Title,
                FileContents = x.FileContent,
                FileExtension = x.FileExtension,
                DateUploaded = x.DateUploaded,
                FileContext = x.FileContext,
                MimeType = x.MimeType,
                IsStandalone = x.IsStandalone
            });
        }
    }
}
