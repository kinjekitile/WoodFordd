using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class NewsGetByUrlQuery : IQuery<NewsModel> {
        public string Url { get; set; }
        public bool IncludePageContent { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class NewsGetByUrlQueryHandler : IQueryHandler<NewsGetByUrlQuery, NewsModel> {
        private readonly INewsService _newsService;
        public NewsGetByUrlQueryHandler(INewsService newsService) {
            _newsService = newsService;
        }

        public NewsModel Process(NewsGetByUrlQuery query) {
            return _newsService.GetByUrl(query.Url, query.IncludePageContent);
        }
    }
}
