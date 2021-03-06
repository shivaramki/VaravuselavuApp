﻿using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace VaravuselavuStandard.Util
{
    public static class Email
    {
        public static Boolean Send(string fromAddress,String toAddress, String username,string password,string appPassword,string appUsername,string subject,string bodyContent)
        {
            Boolean isSuccess = false;
           
            try
            {
                string FromAddress = fromAddress;
                string FromAdressTitle = "Email from Varavuselavuapp";
                string ToAddress = toAddress;
                string ToAdressTitle = "Varavuselavuapp";
				string Subject = subject;
                string BodyContent = bodyContent;

				string SmtpServer = "smtp.gmail.com";
                int SmtpPortNumber = 587;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
                mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
                mimeMessage.Subject = Subject;
                mimeMessage.Body = new TextPart("html")
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
