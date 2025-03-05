using App.Services.Dto.Log;
using App.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace App.Services.Implemantation;

public class EmailService : IEmailService
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
    {        bool retval = false;

      
        return retval;
    }
    public bool SendEmail(string MailTo, string Subject, string Message, string ObjMailBody, string EmailType, string? Reason = null, List<string>? CC = null, List<string>? Bcc = null)
    {
        bool retval = false;
        return retval;

        
    }

}
