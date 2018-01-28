using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class VoucherRepository : RepositoryBase, IVoucherRepository {
        private const string NotFound = "Voucher could not be found";
        public VoucherRepository(IDbConnectionConfig connection) : base(connection) { }

        public VoucherModel Create(VoucherModel model) {
            Voucher v = new Voucher();
            v.RequireClientValidation = model.RequireClientValidation;
            v.ClientName = model.ClientName;
            v.ClientEmail = model.ClientEmail;
            v.DateIssued = model.DateIssued;
            v.DateExpiry = model.DateExpiry;
            v.DateRedeemed = model.DateRedeemed;
            v.ReservationId = model.ReservationId;
            v.VoucherNumber = model.VoucherNumber;
            v.VoucherRewardType =Convert.ToInt32(model.VoucherRewardType);
            v.VoucherDiscount = model.VoucherDiscount;
            v.VoucherDiscountPercentage = model.VoucherDiscountPercentage;
            v.VoucherReward = model.VoucherReward;
            v.IsMultiUse = model.IsMultiUse;
            _db.Vouchers.Add(v);
            _db.SaveChanges();
            model.Id = v.Id;
            return model;
        }

        public List<VoucherModel> Get(VoucherFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            if (filter!= null) {
                if (!string.IsNullOrEmpty(filter.VoucherNumber))
                    list = list.Where(x => x.VoucherNumber.ToLower() == filter.VoucherNumber.ToLower());
                if (filter.IsMultiUse.HasValue) { list = list.Where(x => x.IsMultiUse == filter.IsMultiUse.Value); }
                if (filter.IsRedeemed.HasValue) {
                    if (filter.IsRedeemed.Value) {
                        list = list.Where(x => x.DateRedeemed != null);
                    } else {
                        list = list.Where(x => x.DateRedeemed == null);
                    }                    
                }
                if (filter.VoucherRewardType.HasValue) { list = list.Where(x => x.VoucherRewardType == filter.VoucherRewardType.Value); }
                if (filter.IsExpired.HasValue) {
                    if (filter.IsExpired.Value) {
                        list = list.Where(x => x.DateExpiry <= DateTime.Now);
                    } else {
                        list = list.Where(x => x.DateExpiry > DateTime.Now);
                    }
                    
                }
            }
            
            if (pagination == null) {
                return list.OrderByDescending(x => x.DateIssued).ToList();
            } else {
                return list.OrderByDescending(x => x.DateIssued).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public VoucherModel GetById(int id) {
            VoucherModel v = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (v == null)
                throw new Exception(NotFound);
            return v;
        }

        public VoucherModel GetByVoucherNumber(string voucherNumber) {
            VoucherModel v = getAsIQueryable().Where(x => x.VoucherNumber == voucherNumber).SingleOrDefault();
            if (v == null)
                throw new Exception(NotFound);
            return v;
        }

        public int GetCount(VoucherFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.IsMultiUse.HasValue) { list = list.Where(x => x.IsMultiUse == filter.IsMultiUse.Value); }
                if (filter.IsRedeemed.HasValue) {
                    if (filter.IsRedeemed.Value) {
                        list = list.Where(x => x.DateRedeemed != null);
                    } else {
                        list = list.Where(x => x.DateRedeemed == null);
                    }
                }
                if (filter.VoucherRewardType.HasValue) { list = list.Where(x => x.VoucherRewardType == filter.VoucherRewardType.Value); }
                if (filter.IsExpired.HasValue) {
                    if (filter.IsExpired.Value) {
                        list = list.Where(x => x.DateExpiry <= DateTime.Now);
                    } else {
                        list = list.Where(x => x.DateExpiry > DateTime.Now);
                    }

                }
            }
            return list.Count();
        }

        public VoucherModel Update(VoucherModel model) {
            Voucher v = _db.Vouchers.Where(x => x.Id == model.Id).SingleOrDefault();
            if (v == null)
                throw new Exception(NotFound);
            v.RequireClientValidation = model.RequireClientValidation;
            v.ClientName = model.ClientName;
            v.ClientEmail = model.ClientEmail;
            v.DateIssued = model.DateIssued;
            v.DateExpiry = model.DateExpiry;
            v.DateRedeemed = model.DateRedeemed;
            v.ReservationId = model.ReservationId;
            v.VoucherNumber = model.VoucherNumber;
            v.VoucherRewardType = Convert.ToInt32(model.VoucherRewardType);
            v.VoucherDiscount = model.VoucherDiscount;
            v.VoucherDiscountPercentage = model.VoucherDiscountPercentage;
            v.VoucherReward = model.VoucherReward;
            v.IsMultiUse = model.IsMultiUse;            
            _db.SaveChanges();
            return model;
        }

        private IQueryable<VoucherModel> getAsIQueryable() {
            return _db.Vouchers.Select(x => new VoucherModel {
                Id = x.Id,
                RequireClientValidation = x.RequireClientValidation,
            ClientName = x.ClientName,
            ClientEmail = x.ClientEmail,
            DateIssued = x.DateIssued,
            DateExpiry = x.DateExpiry,
            DateRedeemed = x.DateRedeemed,
            ReservationId = x.ReservationId,
            VoucherNumber = x.VoucherNumber,
            VoucherRewardType = (VoucherRewardType)x.VoucherRewardType,
            VoucherDiscount = x.VoucherDiscount,
            VoucherDiscountPercentage = x.VoucherDiscountPercentage,
            VoucherReward = x.VoucherReward,
            IsMultiUse = x.IsMultiUse
        });
        }
    }
}
