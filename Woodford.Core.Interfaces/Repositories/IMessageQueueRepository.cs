using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
	public interface IMessageQueueRepository {
        List<MessageQueueModel> GetTop(int numberToTake, MessageQueueFilterModel filter);
        List<MessageQueueModel> Get(MessageQueueFilterModel filter, ListPaginationModel pagination);
        int GetCount(MessageQueueFilterModel filter);
        MessageQueueModel GetById(int id);
        MessageQueueModel Update(MessageQueueModel model);
        MessageQueueModel Create(MessageQueueModel model);
        void MarkMessagesAsProcessing(List<MessageQueueModel> messages);
        
        //void Dispose();
    }
}
