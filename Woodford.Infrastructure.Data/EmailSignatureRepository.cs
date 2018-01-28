using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Repositories;

namespace Woodford.Infrastructure.Data {
    public class EmailSignatureRepository : RepositoryBase, IEmailSignatureRepository {

        public EmailSignatureRepository(IDbConnectionConfig connection) : base(connection) { }

        public EmailSignatureModel Create(EmailSignatureModel model) {

            EmailSignature e = new EmailSignature();
            e.Name = model.Name;
            e.CellContact = model.CellContact;
            e.FixedContact = model.FixedContact;
            e.Email = model.Email;
            e.Department = model.Department;

            //No longer db field, is read only now and uses existence of senior and director details
            //e.ShowSidePanel = model.ShowSidePanel;

            e.SeniorName = model.SeniorName;
            e.SeniorCellContact = model.SeniorCellContact;
            e.SeniorFixedContact = model.SeniorFixedContact;
            e.SeniorEmail = model.SeniorEmail;

            e.DirectorName = model.DirectorName;
            e.DirectorEmail = model.DirectorEmail;

            _db.EmailSignatures.Add(e);
            _db.SaveChanges();

            model.Id = e.Id;

            return model;
        }

        public List<EmailSignatureModel> Get(EmailSignatureFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public EmailSignatureModel GetById(int id) {

            EmailSignatureModel model = GetAsIQueryable().Where(x => x.Id == id).SingleOrDefault();

            if (model == null) {
                throw new Exception("Email signature could not be found");
            }


            return model;
        }

        public int GetCount(EmailSignatureFilterModel filter) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public EmailSignatureModel Update(EmailSignatureModel model) {
            EmailSignature e = _db.EmailSignatures.Where(x => x.Id == model.Id).SingleOrDefault();

            if (e == null) {
                throw new Exception("Email Signature could not be found");
            }

            e.Name = model.Name;
            e.CellContact = model.CellContact;
            e.FixedContact = model.FixedContact;
            e.Email = model.Email;
            e.Department = model.Department;

            //No longer db field, is read only now and uses existence of senior and director details
            //e.ShowSidePanel = model.ShowSidePanel;

            e.SeniorName = model.SeniorName;
            e.SeniorCellContact = model.SeniorCellContact;
            e.SeniorFixedContact = model.SeniorFixedContact;
            e.SeniorEmail = model.SeniorEmail;

            e.DirectorName = model.DirectorName;
            e.DirectorEmail = model.DirectorEmail;

            _db.SaveChanges();

            return model;
        }


        private IQueryable<EmailSignatureModel> GetAsIQueryable() {
            return _db.EmailSignatures.Select(x => new EmailSignatureModel {
                Id = x.Id,
                Name = x.Name,
                CellContact = x.CellContact,
                FixedContact = x.FixedContact,
                Email = x.Email,
                Department = x.Department,
                SeniorName = x.SeniorName,
                SeniorCellContact = x.SeniorCellContact,
                SeniorEmail = x.SeniorEmail,
                SeniorFixedContact = x.SeniorFixedContact,
                DirectorName = x.DirectorName,
                DirectorEmail = x.DirectorEmail
                
            });
        }

        private IQueryable<EmailSignatureModel> applyFilter(IQueryable<EmailSignatureModel> list, EmailSignatureFilterModel filter) {

            if (filter != null) {
                if (filter.Id.HasValue) {
                    list = list.Where(x => x.Id == filter.Id.Value);
                }
            }

            return list;
        }
    }
}
