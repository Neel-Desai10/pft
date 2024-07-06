using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RolesDto> GetRoles()
    {
        var data = new RolesDto();
        data.RolesList = await _roleRepository.GetRoleModel();
        return data;
    }
}
