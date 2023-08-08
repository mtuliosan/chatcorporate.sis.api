using Npgsql;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Dapper;
using IdentityService.Domain.Repository;
using Microsoft.Extensions.Options;
using IdentityService.Domain.Config;

namespace IdentityService.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private NpgsqlConnection _conn;


        public BaseRepository(IOptions<ConnectionStrings> config)
        {
        }

        public IDbConnection Connection() 
        {
            if (_conn == null || _conn?.State != ConnectionState.Open)
            {
                _conn = new NpgsqlConnection("");
                _conn.Open();
            }
            return _conn;

        }

        protected async Task<int> ExeAsync(string sql, object param = null, IDbConnection connection = null)
        {
            if (connection == null)
                connection = Connection();

            return await connection.ExecuteAsync(sql, param);
        }
    }
}
