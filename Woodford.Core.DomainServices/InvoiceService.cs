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
    public class InvoiceService : IInvoiceService {
        private readonly IInvoiceRepository _repo;
        private readonly IPaymentTransactionService _paymentTransactionService;
        public InvoiceService(IInvoiceRepository repo, IPaymentTransactionService paymentTransactionService) {
            _repo = repo;
            _paymentTransactionService = paymentTransactionService;
        }

        public InvoiceModel Upsert(InvoiceModel model) {
            InvoiceModel i = GetByReservationId(model.ReservationId);
            if (i == null) {
                i = _repo.Create(model);
            } else {
                if (i.IsCompleted) {
                    throw new Exception("Invoice already completed");
                } else {
                    model.Id = i.Id;
                    i = _repo.Update(model);
                }
            }
            return i;
        }

        public ListOf<InvoiceModel> Get(InvoiceFilterModel filter, ListPaginationModel pagination) {
            ListOf<InvoiceModel> res = new ListOf<InvoiceModel>();
            res.Pagination = pagination;
            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }
            return res;
        }

        public InvoiceModel GetById(int id) {
            return _repo.GetById(id);
        }        

        //public InvoiceModel Update(InvoiceModel model) {
        //    return _repo.Update(model);
        //}

        public InvoiceModel GetByReservationId(int id) {
            var invoices =_repo.Get(new InvoiceFilterModel { ReservationId = id }, null);
            return invoices.FirstOrDefault();
        }

        public InvoiceModel CompleteInvoice(int id) {
            InvoiceModel i = _repo.GetById(id);
            i.IsCompleted = true;
            i.DateCreated = DateTime.Now;
            return _repo.Update(i);
        }
        
    }
}
