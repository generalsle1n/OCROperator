using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OCROperator.Factory
{
    public class MailFactory
    {
        internal string SMTPServer { get;set; }
        internal int Port { get; set; }
        internal string FromMail { get; set; }
        private SmtpClient _smtp;

        private void BuildSmtpClient()
        {
            _smtp = new SmtpClient()
            {
                Host = SMTPServer,
                Port = Port
            };
        }

        internal void SendMail(string subject, string body, string[] To)
        {
            if(_smtp == null) BuildSmtpClient();
            MailMessage Mail = new MailMessage() { 
                From = new MailAddress(FromMail),
                Body = body,
                Subject = subject
            };

            foreach(string s in To)
            {
                Mail.To.Add(new MailAddress(s));
            }
            _smtp.Send(Mail);
        }
    }
}
