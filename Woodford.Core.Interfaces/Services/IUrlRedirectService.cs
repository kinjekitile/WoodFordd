using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IUrlRedirectService {
        UrlRedirectModel Create(UrlRedirectModel model);
        UrlRedirectModel Update(UrlRedirectModel model);
        void Delete(int id);
        UrlRedirectModel GetById(int id);
        UrlRedirectModel GetByUrl(string url);
        ListOf<UrlRedirectModel> Get(UrlRedirectFilterModel filter, ListPaginationModel pagination);
        
    }
}
