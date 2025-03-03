using App.Services.Dto.General;
using App.Services.Dto.Saleman;

namespace App.Services.Interface;

public interface ISalesmanService : ICommonService<SalesmanDto>
{
    Task<ResponseDto> GetAllAsync();
}

