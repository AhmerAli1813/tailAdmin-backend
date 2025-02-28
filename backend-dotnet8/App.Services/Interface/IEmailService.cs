using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Interface
{
    public interface IEmailService
    {
        bool SendEmail(List<string> MailTo, string Subject, string Message, string ObjMailBody, string EmailType, string? Reason = null, List<string>? CC = null, List<string>? Bcc = null);
        bool SendEmail(string MailTo, string Subject, string Message, string ObjMailBody, string EmailType, string? Reason = null, List<string>? CC = null, List<string>? Bcc = null);

    }
}
