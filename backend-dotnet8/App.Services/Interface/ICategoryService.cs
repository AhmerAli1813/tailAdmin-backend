using App.Services.Dto.General;
using App.Services.Dto.Category;

namespace App.Services.Interface;

public interface ICategoryService : ICommonService<CategoryDto>
{
    Task<ResponseDto> GetAllAsync();
    Task<ResponseDto> RecycleItemAsync(string name);

}

