using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Notifications {

    public class EmailQueueService : INotificationQueueService {

        private readonly ISettingService _settings;
        private readonly IMessageQueueRepository _messageQueueRepository;        

        public EmailQueueService(ISettingService settings, IMessageQueueRepository messageQueueRepository) {
            _settings = settings;        
            _messageQueueRepository = messageQueueRepository;
            //_notify = notify;
        }
        
        public void Create(string to, string from, string subject, string bodyHtml, string senderProcess, string cc = "", string bcc = "") {
            MessageQueueModel m = new MessageQueueModel {
                SenderProcess = senderProcess,
                EmailTo = to,
                EmailCc = cc,
                EmailBcc = bcc,
                EmailSubject = subject,
                EmailBody = bodyHtml
            };
            
            _messageQueueRepository.Create(m);
        }

        public void Create(string to, string from, string subject, string bodyHtml, string senderProcess, string cc = "", string bcc = "", string attachmentPath = "") {
            MessageQueueModel m = new MessageQueueModel {
                SenderProcess = senderProcess,
                EmailTo = to,
                EmailCc = cc,
                EmailBcc = bcc,
                EmailSubject = subject,
                EmailBody = bodyHtml,
                HasAttachment = !string.IsNullOrEmpty(attachmentPath),
                AttachmentPath = attachmentPath
                
            };

            _messageQueueRepository.Create(m);
        }

        public void Update(MessageQueueModel model) {            
            _messageQueueRepository.Update(model);
        }

        public MessageQueueModel GetById(int id) {
            return _messageQueueRepository.GetById(id);
        }

        public ListOf<MessageQueueModel> Get(MessageQueueFilterModel filter, ListPaginationModel pagination) {

            ListOf<MessageQueueModel> res = new ListOf<MessageQueueModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _messageQueueRepository.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _messageQueueRepository.GetCount(filter);
                res.Items = _messageQueueRepository.Get(filter, res.Pagination);
            }
            return res;
            //return _messageQueueRepository.Get(filter);
        }

        public void ProcessNotificationQueue() {
            int timesToTry = _settings.GetValue<int>(Setting.Email_Notification_Try_Times);
            int numberToProcess = _settings.GetValue<int>(Setting.Email_Notifications_Number_To_Process);

            List<MessageQueueModel> messages = _messageQueueRepository.GetTop(numberToProcess, new MessageQueueFilterModel { Sent = false, TriedLessThanXTimes = timesToTry });
            //mark all messages as processing in case another thread tries to send mails
            _messageQueueRepository.MarkMessagesAsProcessing(messages);

            foreach (var m in messages) {
                try {
                    //sendMail(m);
                    //_notify.SendMail(m.EmailTo, m.EmailSubject, m.EmailBody, m.EmailCc, m.EmailBcc);
                    Mailer.SendMail(m.EmailTo, m.EmailSubject, m.EmailBody, m.EmailCc, m.EmailBcc, attachmentPath: m.AttachmentPath);

                    m.ErrorMessage = null;
                    m.ErrorOccurred = false;
                    m.Sent = true;
                    m.DateSent = DateTime.Now;
                }
                catch (Exception ex) {
                    m.TimesTried++;
                    m.ErrorOccurred = true;
                    m.ErrorMessage = ex.Message;
                }
                m.Processing = false;
                _messageQueueRepository.Update(m);
            }
        }
    }
}
