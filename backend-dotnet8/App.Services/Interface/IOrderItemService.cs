using App.Services.Dto.General;
using App.Services.Dto.Order;

namespace App.Services.Interface;

public interface IOrderItemService : ICommonService<OrderItemDto>
{
    Task<ResponseDto> GetAllAsync();
}

