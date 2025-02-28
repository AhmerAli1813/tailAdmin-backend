using App.Services.Dto.Log;
using App.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Implemantation;

public class EmailService :IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogService _logService;
    private readonly IHttpContextAccessor _contextAccessor;

    public EmailService(IConfiguration configuration, ILogService logService, IHttpContextAccessor contextAccessor)
    {
        _configuration = configuration;
        _logService = logService;
        _contextAccessor = contextAccessor;
    }

    // Make the SendEmail method non-static
    public bool SendEmail(List<string> MailTo, string Subject, string Message, string ObjMailBody, string EmailType, string? Reason = null, List<string>? CC = null, List<string>? Bcc = null)
    {
        bool retval = false;

        string? sSmtpUser = _configuration.GetValue<string>("smtpUser");// "apikey";
        string? sSmtpPass = _configuration.GetValue<string>("smtpPass");// "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";
                                                                        // string sSmtpIPAddress = "mail.jsil.com";
        string sEmailFrom = "no-reply@jsil.com";

        //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .NET 4.0           
        MailMessage message = new MailMessage();
        SmtpClient smtpClient = new SmtpClient();

        string msg = string.Empty;
        try
        {
            MailAddress fromAddress = new MailAddress(sEmailFrom, "JS Investments");
            message.From = fromAddress;

            // Add To, CC, and BCC

            List<string> EmailSend = new List<string>();

            if (MailTo != null && MailTo.Count > 0)
            {
                foreach (string item in MailTo)
                {
                    EmailSend.Add(item);
                    message.To.Add(item);
                }
            }

            if (CC != null && CC.Count > 0)
            {
                foreach (string item in CC)
                {
                    EmailSend.Add(item);
                    message.CC.Add(item);
                }
            }

            if (Bcc != null && Bcc.Count > 0)
            {
                foreach (string item in Bcc)
                {
                    EmailSend.Add(item);
                    message.Bcc.Add(item);
                }
            }

            message.Subject = Subject;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Body = ObjMailBody;

            smtpClient.Host = "smtp.sendgrid.net";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(sSmtpUser, sSmtpPass);

            smtpClient.Send(message);
            retval = true;


        }
        catch (Exception ex)
        {
            retval = false;
            throw new Exception(ex.Message);


        }
        finally
        {
            OpenEmailLogDto log = new OpenEmailLogDto()
            {
                AttachmentsPath = "",
                Attempts = true,
                NotificationSendTime = DateTime.Now,
                NotificationRecordDate = DateTime.Now,
                NotificationSendCc = CC != null ? CC.ToString() : null,
                NotificationSendTo = MailTo.ToList().ToString(),
                NotificationSubject = Subject,
                NotificationText = Message,
                SendingStatus = retval ? "S" : "F",
                NotificationTransType = EmailType,
                Reason = Reason,
            };
            _logService.SaveEmailLog(_contextAccessor.HttpContext, log);
        }

        return retval;
    }
    public bool SendEmail(string MailTo, string Subject, string Message, string ObjMailBody, string EmailType, string? Reason = null, List<string>? CC = null, List<string>? Bcc = null)
    {
        bool retval = false;

        string? sSmtpUser = _configuration.GetValue<string>("smtpUser");
        string? sSmtpPass = _configuration.GetValue<string>("smtpPass");
        string sEmailFrom = "no-reply@jsil.com";

        //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // .NET 4.0           
        MailMessage message = new MailMessage();
        SmtpClient smtpClient = new SmtpClient();

        string msg = string.Empty;
        try
        {
            MailAddress fromAddress = new MailAddress(sEmailFrom, "JS Investments");
            message.From = fromAddress;

            // Add To, CC, and BCC

            List<string> EmailSend = new List<string>();

            if (string.IsNullOrWhiteSpace(MailTo))
                throw new Exception("Mail to is not defined");



            EmailSend.Add(MailTo);
            message.To.Add(MailTo);

            if (CC != null && CC.Count > 0)
            {
                foreach (string item in CC)
                {
                    EmailSend.Add(item);
                    message.CC.Add(item);
                }
            }

            if (Bcc != null && Bcc.Count > 0)
            {
                foreach (string item in Bcc)
                {
                    EmailSend.Add(item);
                    message.Bcc.Add(item);
                }
            }

            message.Subject = Subject;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Body = ObjMailBody;

            smtpClient.Host = "smtp.sendgrid.net";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(sSmtpUser, sSmtpPass);

            smtpClient.Send(message);
            retval = true;


        }
        catch (Exception ex)
        {
            retval = false;
            throw new Exception(ex.Message);

        }
        finally
        {
            OpenEmailLogDto log = new OpenEmailLogDto()
            {
                AttachmentsPath = "",
                Attempts = true,
                NotificationSendTime = DateTime.Now,
                NotificationRecordDate = DateTime.Now,
                NotificationSendCc = CC != null ? CC.ToString() : null,
                NotificationSendTo = MailTo,
                NotificationSubject = Subject,
                NotificationText = Message,
                SendingStatus = retval ? "S" : "F",
                NotificationTransType = EmailType,
                Reason = Reason,
            };
            _logService.SaveEmailLog(_contextAccessor.HttpContext, log);
        }

        return retval;
    }

}
