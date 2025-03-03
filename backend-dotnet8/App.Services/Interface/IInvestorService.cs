using App.Services.Dto.General;
using App.Services.Dto.Investment;

namespace App.Services.Interface;

public interface IInvestorService : ICommonService<InvestorDto>
{
    Task<ResponseDto> GetAllAsync();

}

