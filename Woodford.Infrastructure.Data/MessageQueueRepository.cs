using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class MessageQueueRepository : RepositoryBase, IMessageQueueRepository {

        public MessageQueueRepository(IDbConnectionConfig connection) : base(connection) { }

        private IQueryable<MessageQueueModel> GetAsIQueryable() {
            return _db.MessageQueues.Select(x => new MessageQueueModel {
                Id = x.Id,
                SenderProcess = x.SenderProcess,
                EmailTo = x.EmailTo,
                EmailCc = x.EmailCc,
                EmailBcc = x.EmailBcc,
                EmailSubject = x.EmailSubject,
                EmailBody = x.EmailBody,
                Sent = x.Sent,
                Processing = x.Processing,
                DateSent = x.DateSent,
                TimesTried = x.TimesTried,
                ErrorOccurred = x.ErrorOccurred,
                ErrorMessage = x.ErrorMessage,
                DateCreated = x.DateCreated,
                HasAttachment = x.HasAttachment,
                AttachmentPath = x.AttachementPath
            });
        }

        private IQueryable<MessageQueueModel> filterMessages(IQueryable<MessageQueueModel> list, MessageQueueFilterModel filter) {
            if (filter != null) {
                if (filter.Id.HasValue) { list = list.Where(x => x.Id == filter.Id.Value); }
                if (filter.Sent.HasValue) { list = list.Where(x => x.Sent == filter.Sent.Value); }
                if (!string.IsNullOrEmpty(filter.EmailTo)) { list = list.Where(x => x.EmailTo == filter.EmailTo); }
                if (filter.ErrorOccurred.HasValue) { list = list.Where(x => x.ErrorOccurred.Value); }
                if (filter.TriedLessThanXTimes.HasValue) { list = list.Where(x => x.TimesTried <= filter.TriedLessThanXTimes.Value); }
            }
            return list;
        }

        public List<MessageQueueModel> Get(MessageQueueFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsIQueryable();
            list = filterMessages(list, filter);

            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public int GetCount(MessageQueueFilterModel filter) {
            var list = GetAsIQueryable();
            list = filterMessages(list, filter);
            return list.Count();
        }

        public MessageQueueModel GetById(int id) {
            return GetAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
        }

        public List<MessageQueueModel> GetTop(int numberToTake, MessageQueueFilterModel filter) {
            var list = GetAsIQueryable();
            return filterMessages(list, filter).Take(numberToTake).ToList();
        }

        public MessageQueueModel Update(MessageQueueModel model) {
            MessageQueue m = _db.MessageQueues.Where(x => x.Id == model.Id).SingleOrDefault();
            if (m == null) {
                throw new Exception("Email Message cannot be found " + model.Id);
            } else {
                m.SenderProcess = model.SenderProcess;
                m.EmailTo = m.EmailTo;
                m.EmailCc = model.EmailCc;
                m.EmailBcc = model.EmailBcc;
                m.EmailSubject = model.EmailSubject;
                m.EmailBody = model.EmailBody;
                m.Sent = model.Sent;
                m.Processing = model.Processing;
                m.TimesTried = model.TimesTried;
                if (model.DateSent.HasValue) {
                    m.DateSent = model.DateSent.Value;
                }
                if (model.ErrorOccurred.HasValue) {
                    m.ErrorOccurred = model.ErrorOccurred.Value;
                }
                m.ErrorMessage = model.ErrorMessage;
                m.HasAttachment = model.HasAttachment;
                m.AttachementPath = model.AttachmentPath;

                _db.SaveChanges();
            }
            return model;
        }

        public MessageQueueModel Create(MessageQueueModel model) {
            MessageQueue m = new MessageQueue();
            m.SenderProcess = model.SenderProcess;
            m.EmailTo = model.EmailTo;
            m.EmailCc = model.EmailCc;
            m.EmailBcc = model.EmailBcc;
            m.EmailSubject = model.EmailSubject;
            m.EmailBody = model.EmailBody;
            m.Sent = false;
            m.Processing = false;
            m.TimesTried = 0;
            m.DateCreated = DateTime.Now;
            m.HasAttachment = model.HasAttachment;
            m.AttachementPath = model.AttachmentPath;

            _db.MessageQueues.Add(m);
            _db.SaveChanges();
            model.Id = m.Id;
            return model;
        }

        public void MarkMessagesAsProcessing(List<MessageQueueModel> messages) {
            List<int> ids = messages.Select(x => x.Id).ToList();
            List<MessageQueue> dbMessages = _db.MessageQueues.Where(x => ids.Contains(x.Id)).ToList();
            foreach (var dbMessage in dbMessages) {
                dbMessage.Processing = true;
            }
            _db.SaveChanges();
        }
    }
}
