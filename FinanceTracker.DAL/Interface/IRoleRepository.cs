namespace FinanceTracker.DAL.Interface;

public interface IRoleRepository
{
    Task<List<RoleModel>> GetRoleModel();
}
