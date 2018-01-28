using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    //public class SpecialRepository : RepositoryBase, ISpecialRepository {

    //    private const string SpecialNotFound = "Special could not be found";

    //    public SpecialModel Create(SpecialModel model) {
    //        Special s = new Special();
    //        s.Title = model.Title;
    //        s.Description = model.Description;
    //        s.StartDate = model.StartDate;
    //        s.EndDate = model.EndDate;

    //        _db.Specials.Add(s);
    //        _db.SaveChanges();
    //        model.Id = s.Id;
    //        return model;
    //    }

    //    public List<SpecialModel> Get(SpecialFilterModel filter) {
    //        var list = getByIQueryable();
    //        if (filter != null) {
    //            if (filter.Id.HasValue) list = list.Where(x => x.Id == filter.Id.Value);
    //            if (filter.StartDate.HasValue) list = list.Where(x => x.StartDate < filter.StartDate);
    //            if (filter.EndDate.HasValue) list = list.Where(x => x.EndDate > filter.EndDate);
    //        }
    //        return list.ToList();
    //    }

    //    public SpecialModel GetById(int id) {
    //        SpecialModel s = getByIQueryable().Where(x => x.Id == id).SingleOrDefault();
    //        if (s == null)
    //            throw new Exception(SpecialNotFound);

    //        return s;
    //    }

    //    public SpecialModel Update(SpecialModel model) {
    //        Special s = _db.Specials.Where(x => x.Id == model.Id).SingleOrDefault();
    //        if (s == null)
    //            throw new Exception(SpecialNotFound);

    //        s.Title = model.Title;
    //        s.Description = model.Description;
    //        s.StartDate = model.StartDate;
    //        s.EndDate = model.EndDate;
    //        _db.SaveChanges();
    //        return model;            
    //    }

    //    private IQueryable<SpecialModel> getByIQueryable() {
    //        return _db.Specials.Select(x => new SpecialModel {
    //            Id = x.Id,
    //            Title = x.Title,
    //            Description = x.Description,
    //            StartDate = x.StartDate,
    //            EndDate = x.EndDate
    //        });
    //    }
    //}
}
