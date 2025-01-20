
using App.DataAccessLayer.EntityModel.SQL.Data;
using App.DataAccessLayer.EntityModel.SQL.Model;
using App.Infrastructure;
using App.Services.Dto.General;
using App.Services.Dto.Message;
using App.Services.Helper;
using App.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace App.Services.Implemantation;

public class MessageService : IMessageService
{
    #region Constructor & DI
    private readonly IUnitOfWork<JSIL_IdentityDbContext> _context;
    private readonly ILogService _logService;
    private readonly UserManager<ApplicationUser> _userManager;

    public MessageService(IUnitOfWork<JSIL_IdentityDbContext> context, ILogService logService, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _logService = logService;
        _userManager = userManager;
    }

    #endregion

    #region CreateNewMessageAsync
    public async Task<ResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto)
    {
        if (User.Identity.Name == createMessageDto.ReceiverUserName)
            return new ResponseDto()
            {
                IsSucceed = false,
                StatusCode = 400,
                Message = "Sender and Receiver can not be same",
            };

        var isReceiverUserNameValid = _userManager.Users.Any(q => q.UserName == createMessageDto.ReceiverUserName);
        if (!isReceiverUserNameValid)
            return new ResponseDto()
            {
                IsSucceed = false,
                StatusCode = 400,
                Message = "Receiver UserName is not valid",
            };

        Message newMessage = new Message()
        {
            SenderUserName = User.Identity.Name,
            ReceiverUserName = createMessageDto.ReceiverUserName,
            Text = createMessageDto.Text
        };
        await _context.GenericRepository<Message>().AddAsync(newMessage);
        await _context.SaveAsync();
        await _logService.SaveNewLog(User.Identity.Name, "Send Message");

        return new ResponseDto()
        {
            IsSucceed = true,
            StatusCode = 201,
            Message = "Message saved successfully",
        };
    }
    #endregion

    #region GetMessagesAsync
    public async Task<IEnumerable<GetMessageDto>> GetMessagesAsync()
    {
        var messages = await _context.GenericRepository<Message>().GetAllAsync(orderby: q => q.OrderByDescending(x => x.CreatedAt));
        return ModelConverter.ConvertTo<Message, GetMessageDto>(messages);

    }
    #endregion

    #region GetMyMessagesAsync
    public async Task<IEnumerable<GetMessageDto>> GetMyMessagesAsync(ClaimsPrincipal User)
    {
        var loggedInUser = User.Identity.Name;

        var messages = await _context.GenericRepository<Message>().GetAllAsync(filter: x => x.SenderUserName == loggedInUser || x.ReceiverUserName == loggedInUser, orderby: x => x.OrderByDescending(a => a.CreatedAt));
        return ModelConverter.ConvertTo<Message, GetMessageDto>(messages);

    }
    #endregion
}
