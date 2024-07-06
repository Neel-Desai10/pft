using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface;

public interface IRoleService
{
     Task<RolesDto> GetRoles();
}
