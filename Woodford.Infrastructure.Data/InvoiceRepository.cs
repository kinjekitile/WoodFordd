using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Infrastructure.Data {
    public class InvoiceRepository : RepositoryBase, IInvoiceRepository {
        private const string InvoiceNotFound = "Invoice not found";
        public InvoiceRepository(IDbConnectionConfig connection) : base(connection) {

        }

        public InvoiceModel Create(InvoiceModel model) {
            Invoice i = new Invoice();
            i.ReservationId = model.ReservationId;
            i.DateCreated = DateTime.Now;
            i.AmountPaid = model.AmountPaid;
            i.IsMobileCheckout = model.IsMobileCheckout;
            i.IsCorporateCheckout = model.IsCorporateCheckout;
            i.IsCompleted = model.IsCompleted;

            _db.Invoices.Add(i);
            _db.SaveChanges();

            model.Id = i.Id;

            return model;
        }

        public List<InvoiceModel> Get(InvoiceFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();

            list = applyFilter(list, filter);

            return list.ToList();
        }

        public InvoiceModel GetById(int id) {
            InvoiceModel i = getAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (i == null)
                throw new Exception(InvoiceNotFound);
            return i;
        }

        public InvoiceModel GetByReservationId(int reservationId) {            
            InvoiceModel i = getAsIQueryable().SingleOrDefault(x => x.ReservationId == reservationId);            
            return i;
        }

        public int GetCount(InvoiceFilterModel filter) {
            var list = getAsIQueryable();

            list = applyFilter(list, filter);

            return list.Count();
        }

        public InvoiceModel Update(InvoiceModel model) {
            Invoice i = _db.Invoices.SingleOrDefault(x => x.Id == model.Id);
            if (i == null)
                throw new Exception(InvoiceNotFound);

            i.AmountPaid = model.AmountPaid;
            i.IsCompleted = model.IsCompleted;
            i.IsMobileCheckout = model.IsMobileCheckout;
            i.IsCorporateCheckout = model.IsCorporateCheckout;

            _db.SaveChanges();

            return model;
        }

        private IQueryable<InvoiceModel> applyFilter(IQueryable<InvoiceModel> list, InvoiceFilterModel filter) {
            if (filter != null) {
                if (filter.ReservationId.HasValue)
                    list = list.Where(x => x.ReservationId == filter.ReservationId.Value);
                if (filter.IsMobileCheckout.HasValue)
                    list = list.Where(x => x.IsMobileCheckout == filter.IsMobileCheckout.Value);
                if (filter.IsCorporateCheckout.HasValue)
                    list = list.Where(x => x.IsCorporateCheckout == filter.IsCorporateCheckout.Value);
            }
            return list;
        }

        private IQueryable<InvoiceModel> getAsIQueryable() { 
           return _db.Invoices.Select(x => new InvoiceModel {
                Id = x.Id,
                ReservationId = x.ReservationId,
                DateCreated = x.DateCreated,
                AmountPaid = x.AmountPaid,
                IsMobileCheckout = x.IsMobileCheckout,
                IsCorporateCheckout = x.IsCorporateCheckout,
                IsCompleted = x.IsCompleted,
                HasPayment = x.MyGateTransactions.FirstOrDefault() == null ? false : x.MyGateTransactions.FirstOrDefault().MyGateTransactionID.HasValue ? true : false
            });
        }

    }
}
