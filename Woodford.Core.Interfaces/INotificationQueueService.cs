using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface INotificationQueueService {

        void Create(string to, string from, string subject, string bodyHtml, string senderProcess, string cc = "", string bcc = "");
        void Create(string to, string from, string subject, string bodyHtml, string senderProcess, string cc = "", string bcc = "", string attachmentPath = "");
        void Update(MessageQueueModel model);
        MessageQueueModel GetById(int id);
        ListOf<MessageQueueModel> Get(MessageQueueFilterModel filter, ListPaginationModel pagination);
        void ProcessNotificationQueue();
        
    }
}
