using App.Services.Dto.Log;
using App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogService _logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EmailController(IEmailService emailService, ILogService logger, IHttpContextAccessor httpContextAccessor)
        {
            _emailService = emailService;
            _logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("send-email")]
        [AllowAnonymous]
        public async Task<IActionResult> sendEmail()
        {
            try
            {

                var a = _emailService.SendEmail("ahmer.ali@jsil.com", "testing email", "o bhai email", "o bhai email a gae", "testing email");
                return Ok(a);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPost("send-email-log")]
        [AllowAnonymous]
        public async Task<IActionResult> sendEmaillog()
        {
            try
            {
                OpenEmailLogDto a = new OpenEmailLogDto()
                {
                    AttachmentsPath = "d",
                    Attempts = true,
                    NotificationRecordDate = DateTime.Now,
                    NotificationSendBC = "a",
                    NotificationSendCc = "b",
                    NotificationSendTime = DateTime.Now,
                    NotificationSendTo = "c",
                    NotificationSubject = "d",
                    NotificationText = "d",
                    NotificationTransType = "d",
                    Reason = "e",
                    SendingCount = "f",
                    SendingStatus = "g",
                    UserName = "g",

                };

                var aa = _logger.SaveEmailLog(httpContextAccessor.HttpContext, a);
                return Ok(aa);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
    }
}
