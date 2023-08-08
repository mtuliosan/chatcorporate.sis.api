using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using IdentityService.Domain;
using IdentityService.Domain.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Repository
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> ListAsync(IEnumerable<Guid> roleIds = null);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role, Guid id);
    }
    public class RoleRepository: BaseRepository, IRoleRepository
    {
        private string _connectionStrings;

        public RoleRepository(IOptions<ConnectionStrings> config) : base(config) {
            _connectionStrings = config.Value.IdentityServerConnection;
        }

        public async Task AddAsync(Role role)
        {
            var sql = "insert into Roles(Id, Name, Value)values(@id, @name, @value)";

            using var conn = new NpgsqlConnection(_connectionStrings);
            await conn.ExecuteAsync(sql, role);
        }

        public async Task<IEnumerable<Role>> ListAsync(IEnumerable<Guid> roleIds = null)
        {
            var sql = "select * from Roles ";

            if (roleIds != null && roleIds.Any())
                sql += " where id = ANY(@ids) ";

            sql += " order by Name";

            using var conn = new NpgsqlConnection(_connectionStrings);
            return await conn.QueryAsync<Role>(sql, new { ids = roleIds?.ToArray() });
        }

        public async Task UpdateAsync(Role role, Guid id)
        {   
            var sql = "update Roles set Name=@name, Value=@value where Id=@id";

            using var conn = new NpgsqlConnection(_connectionStrings);
            await conn.ExecuteAsync(sql, new { role.Name, role.Value, id });
        }
    }
}
