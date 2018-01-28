using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class NewsGetByIdQuery : IQuery<NewsModel> {
        public int Id { get; set; }
        public bool IncludePageContent { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class NewsGetByIdQueryHandler : IQueryHandler<NewsGetByIdQuery, NewsModel> {
        private readonly INewsService _newsService;
        public NewsGetByIdQueryHandler(INewsService newsService) {
            _newsService = newsService;
        }

        public NewsModel Process(NewsGetByIdQuery query) {
            return _newsService.GetById(query.Id, query.IncludePageContent);
        }
    }
}
