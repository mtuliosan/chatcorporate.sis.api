using System.Data;

namespace IdentityService.Domain.Repository
{
    public interface IBaseRepository
    {
        IDbConnection Connection();
    }
}
