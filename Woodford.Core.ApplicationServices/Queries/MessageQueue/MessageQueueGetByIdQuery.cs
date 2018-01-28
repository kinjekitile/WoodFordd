using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class MessageQueueGetByIdQuery : IQuery<MessageQueueModel> {
        public int Id { get; set; }
    }

    public class MessageQueueGetByIdQueryHandler : IQueryHandler<MessageQueueGetByIdQuery, MessageQueueModel> {
        private readonly INotificationQueueService _queue;
        public MessageQueueGetByIdQueryHandler(INotificationQueueService queue) {
            _queue = queue;
        }

        public MessageQueueModel Process(MessageQueueGetByIdQuery query) {
            return _queue.GetById(query.Id);
        }
    }
}
