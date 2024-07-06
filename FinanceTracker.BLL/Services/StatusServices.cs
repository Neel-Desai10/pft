using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services;

public class StatusServices : IStatusServices
{
    private readonly IStatusRepository _statusRepository;
    public StatusServices(IStatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }


    public async Task<StatusModelDto> GetStatuses()
    {
      var data = new StatusModelDto();
      data.StatusList = await _statusRepository.GetStatus();
      return data;
    }
}
