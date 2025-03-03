using App.Services.Dto.General;
using App.Services.Dto.Product;

namespace App.Services.Interface;

public interface IProductService : ICommonService<ProductDto>
{
    Task<ResponseDto> GetAllAsync();
    Task<ResponseDto> RecycleItemAsync(string name);

}

