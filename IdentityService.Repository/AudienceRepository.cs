using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using IdentityService.Domain;
using IdentityService.Domain.Config;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace IdentityService.Repository
{
    public interface IAudienceRepository
    {
        Task<Audience> GetAsync(Guid? idAudience);
        Task<string> GetKeyAsync(Guid? idAudience);
        Task<IEnumerable<Audience>> ListAsync();
        Task AddAsync(Audience audience);
        Task UpdateAsync(Audience audience, Guid id);
    }
    public class AudienceRepository : BaseRepository, IAudienceRepository
    {
        private string _connectionStrings;

        public AudienceRepository(IOptions<ConnectionStrings> config) : base(config)
        {
            _connectionStrings = config.Value.IdentityServerConnection;
        }

        public async Task AddAsync(Audience audience)
        {
            var sql = "insert into Audiences(Id, Name, Key)values(@id, @name, @key)";
            using var conn = new NpgsqlConnection(_connectionStrings);

            await conn.ExecuteAsync(sql, audience);
        }

        public async Task<Audience> GetAsync(Guid? id)
        {
            var sql = "select * from audiences where Id = @id";

            using var conn = new NpgsqlConnection(_connectionStrings);
            var audience = await conn.QueryFirstOrDefaultAsync<Audience>(sql, new { id });

            return audience;
        }

        public async Task<string> GetKeyAsync(Guid? id)
        {
            var audience = await GetAsync(id);

            return audience?.Key;
        }

        public async Task<IEnumerable<Audience>> ListAsync()
        {
            using var conn = new NpgsqlConnection(_connectionStrings);
            return await conn.QueryAsync<Audience>("select * from Audiences order by Name");
        }

        public async Task UpdateAsync(Audience audience, Guid id)
        {
            var sql = "update Audiences set Name=@name, Key=@key where Id=@id";

            using var conn = new NpgsqlConnection(_connectionStrings);
            await conn.ExecuteAsync(sql, new { audience.Name, audience.Key, id });
        }
    }
}
