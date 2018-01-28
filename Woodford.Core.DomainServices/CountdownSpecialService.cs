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
    public class CountdownSpecialService : ICountdownSpecialService {

        private ICountdownSpecialRepository _repo;
        public CountdownSpecialService(ICountdownSpecialRepository repo) {
            _repo = repo;
        }

        private void cleanCountdownSpecialInput(CountdownSpecialModel model) {
            switch (model.SpecialType) {
                //case CountdownSpecialType.DiscountAmount:
                //    model.VehicleUpgradeAmountOverride = null;
                //    model.VehicleUpgradeId = null;
                //    model.OfferText = "";
                //    break;
                case CountdownSpecialType.TextOnInvoice:
                    model.OfferDiscount = null;
                    model.VehicleUpgradeId = null;
                    model.VehicleUpgradeAmountOverride = null;
                    break;

                case CountdownSpecialType.VehicleUpgrade:
                    model.OfferDiscount = null;
                    model.OfferText = "";
                    break;
            }
        }

        public CountdownSpecialModel Create(CountdownSpecialModel model) {
            cleanCountdownSpecialInput(model);
            return _repo.Create(model);
        }

        public ListOf<CountdownSpecialModel> Get(CountdownSpecialFilterModel filter, ListPaginationModel pagination) {
            ListOf<CountdownSpecialModel> res = new ListOf<CountdownSpecialModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;
        }

        public CountdownSpecialModel GetById(int id) {
            return _repo.GetById(id);
        }

        public CountdownSpecialModel Update(CountdownSpecialModel model) {
            cleanCountdownSpecialInput(model);
            return _repo.Update(model);
        }

        public CountdownSpecialModel MarkAs(int id, bool markAs) {
            CountdownSpecialModel c = _repo.GetById(id);
            c.IsActive = markAs;
            c = _repo.Update(c);
            return c;
        }
    }
}
