using App.Services.Dto.Dispatch;
using App.Services.Dto.General;

namespace App.Services.Interface;

public interface IDispatchService : ICommonService<DispatchDto>
{
    Task<ResponseDto> GetAllAsync();
}

