using App.DataAccessLayer.EntityModel.SQL.Model;
using App.Services.Dto.Investment;

namespace App.Services.Interface;

public interface IInvestmentService : ICommonService<InvestmentDto>
{
    Task<IEnumerable<Investment>> GetAllAsync();

}
