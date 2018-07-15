using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace VaravuselavuStandard.Util
{
    static class Email
    {
        public static Boolean Send(string fromAddress,String toAddress, String username,string password)
        {
            Boolean isSuccess = false;
            try
            {
                string FromAddress = fromAddress;
                string FromAdressTitle = "Email from ASP.NET Core 1.1";
                string ToAddress = toAddress;
                string ToAdressTitle = "Microsoft ASP.NET Core";
                string Subject = "Hello World - Sending email using ASP.NET Core 1.1";
                string BodyContent = "ASP.NET Core was previously called ASP.NET 5. It was renamed in January 2016. It supports cross-platform frameworks ( Windows, Linux, Mac ) for building modern cloud-based internet-connected applications like IOT, web apps, and mobile back-end.";

                string SmtpServer = "smtp.gmail.com";
                int SmtpPortNumber = 587;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
                mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
                mimeMessage.Subject = Subject;
                mimeMessage.Body = new TextPart("plain")
                {
                    Text = BodyContent

                };

                using (var client = new SmtpClient())
                {

                    client.Connect(SmtpServer, SmtpPortNumber, false);
                    client.Authenticate(username, password);
                    client.Send(mimeMessage);
                    client.Disconnect(true);

                }

                isSuccess = true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }

            return isSuccess;
           
        }
    }
}
