using App.Services.Dto.General;

namespace App.Services.Interface;

public interface ICommonService<T> where T : class
{
    Task<ResponseDto> GetByIdAsync(int id);
    Task<ResponseDto> CreateAsync(T model);
    Task<ResponseDto> UpdateAsync(int id, T model);
    Task<ResponseDto> DeleteAsync(int id);
    Task<ResponseDto> ActiveAsync(int id, bool Active);
}

