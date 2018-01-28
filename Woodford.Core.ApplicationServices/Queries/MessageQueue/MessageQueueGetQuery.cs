using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class MessageQueueGetQuery : IQuery<ListOf<MessageQueueModel>> {
        public MessageQueueFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class MessageQueueGetQueryHandler : IQueryHandler<MessageQueueGetQuery, ListOf<MessageQueueModel>> {
        private readonly INotificationQueueService _queue;
        public MessageQueueGetQueryHandler(INotificationQueueService queue) {
            _queue = queue;
        }

        public ListOf<MessageQueueModel> Process(MessageQueueGetQuery query) {
            return _queue.Get(query.Filter, query.Pagination);
        }
    }
}
