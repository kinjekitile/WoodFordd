using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class MessageQueueModel {
        public int Id { get; set; }
        public string SenderProcess { get; set; }
        public string EmailTo { get; set; }
        public string EmailCc { get; set; }
        public string EmailBcc { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public bool HasAttachment { get; set; }
        public string AttachmentPath { get; set; }
        public bool Sent { get; set; }
        public bool Processing { get; set; }
        public DateTime? DateSent { get; set; }
        public int TimesTried { get; set; }
        public bool? ErrorOccurred { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class MessageQueueFilterModel {
        public int? Id { get; set; }
        public string EmailTo { get; set; }
        public bool? ErrorOccurred { get; set; }
        public bool? Sent { get; set; }

        public int? TriedLessThanXTimes { get; set; }
    }
}
