
using App.Services.Dto.General;
using App.Services.Dto.Message;
using System.Security.Claims;

namespace App.Services.Interface;

public interface IMessageService
{
    Task<ResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto);
    Task<IEnumerable<GetMessageDto>> GetMessagesAsync();
    Task<IEnumerable<GetMessageDto>> GetMyMessagesAsync(ClaimsPrincipal User);
}
