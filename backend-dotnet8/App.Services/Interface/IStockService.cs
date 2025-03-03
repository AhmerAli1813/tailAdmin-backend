using App.Services.Dto.General;
using App.Services.Dto.Product;

namespace App.Services.Interface;

public interface IStockService : ICommonService<StockDto>
{
    Task<ResponseDto> GetAllAsync();
}

