using App.Services.Dto.General;
using App.Services.Dto.Product;

namespace App.Services.Interface;

public interface IPriceManagementService : ICommonService<PriceManagementDto>
{
    Task<ResponseDto> GetAllAsync();
}

