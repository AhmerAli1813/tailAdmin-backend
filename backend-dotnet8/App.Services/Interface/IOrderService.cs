using App.Services.Dto.General;
using App.Services.Dto.Order;

namespace App.Services.Interface;

public interface IOrderService : ICommonService<OrderDto>
{
    Task<ResponseDto> GetAllAsync();
}

