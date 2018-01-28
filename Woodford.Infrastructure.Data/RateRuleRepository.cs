using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class RateRuleRepository : RepositoryBase, IRateRuleRepository {

        private static string RateRuleNotFound = "Rate Rule could not be found";

        public RateRuleRepository(IDbConnectionConfig connection) : base(connection) { }
        public RateRuleModel Create(RateRuleModel model) {
            RateRule r = new RateRule();
            r.Title = model.Title;
            r.DaysOfWeek = model.DaysOfWeek;
            r.MinDays = model.MinDays;
            r.MaxDays = model.MaxDays;
            
            

            _db.RateRules.Add(r);
            _db.SaveChanges();

            model.Id = r.Id;

            return model;
        }

        public List<RateRuleModel> Get(RateRuleFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (!string.IsNullOrEmpty(filter.Title)) {
                    list = list.Where(x => x.Title.Contains(filter.Title));
                }
            }

            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }            
        }

        public int GetCount(RateRuleFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
               if (!string.IsNullOrEmpty(filter.Title)) {
                    list = list.Where(x => x.Title.Contains(filter.Title));
                }
            }
            return list.Count();
        }

        public RateRuleModel GetById(int id) {
            RateRuleModel rule = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (rule == null)
                throw new Exception(RateRuleNotFound);
            return rule;
        }

        public RateRuleModel Update(RateRuleModel model) {
            RateRule r = _db.RateRules.SingleOrDefault(x => x.Id == model.Id);
            if (r == null)
                throw new Exception(RateRuleNotFound);

            r.Title = model.Title;
            r.DaysOfWeek = model.DaysOfWeek;
            r.MinDays = model.MinDays;
            r.MaxDays = model.MaxDays;
            
            

            _db.SaveChanges();

            return model;
        }

        private IQueryable<RateRuleModel> getAsIQueryable() {
            return _db.RateRules.Select(x => new RateRuleModel {
                Id = x.Id,
                Title = x.Title,
                DaysOfWeek = x.DaysOfWeek,
                MinDays = x.MinDays,
                MaxDays = x.MaxDays
            });
        }
    }
}
