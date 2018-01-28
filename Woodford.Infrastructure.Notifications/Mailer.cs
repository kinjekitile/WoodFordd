using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Infrastructure.Notifications {
    public static class Mailer {

        public static void SendMail(string to, string subject, string emailBody, string cc = "", string bcc = "", string attachmentPath = "") {
            string[] emailsTo = to.Replace(';', '.').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            using (SmtpClient client = new SmtpClient()) {
                using (MailMessage message = new MailMessage()) {

                    if (string.IsNullOrEmpty(to)) {
                        throw new Exception("parameter: 'to' must contain at least one email address");
                    }
                    foreach (string t in emailsTo) {
                        message.To.Add(new MailAddress(t));
                    }

                    if (!string.IsNullOrEmpty(cc)) message.CC.Add(cc);
                    if (!string.IsNullOrEmpty(bcc)) message.Bcc.Add(bcc);

                    if (!string.IsNullOrEmpty(attachmentPath)) {
                        message.Attachments.Add(new Attachment(attachmentPath));
                    }
                    message.Subject = subject;
                    message.Body = emailBody;
                    message.IsBodyHtml = true;
                    client.Send(message);
                }
            }
        }



    }
}
