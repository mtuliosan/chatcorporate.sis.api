using Dapper;
using IdentityService.Domain;
using IdentityService.Domain.Config;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Repository
{
    public class SessionRepository : BaseRepository, ISessionRepository
    {
        private string _connectionStrings;
        private string _token;

        public SessionRepository(IOptions<ConnectionStrings> config, IOptions<IdentityServerConfigs> identityConfig  ) : base(config)
        {
            _connectionStrings = config.Value.IdentityServerConnection;
            _token = identityConfig.Value.Token;

        }

        public async Task<AppSession> GetOrCreateActiveSession()
        {
            var sql = $"select * from session where isActived = true";
            using var conn = new NpgsqlConnection(_connectionStrings);
            
            var appSession = await conn.QueryFirstOrDefaultAsync<AppSession>(sql);

            if (appSession == null)
            {
                appSession = new AppSession() {
                    Token = _token,
                    ApiKey = Guid.NewGuid(),
                    CreateAt = DateTime.Now,
                    IsActived = true,
                    Session = Guid.NewGuid()
                };

              
                var insertSession = "insert into session(session, apiKey,token, createAt, isActived)values(@session, @apiKey,@token, @createAt, @isActived)";
                await conn.ExecuteAsync(sql, new { appSession.Session, appSession.ApiKey, appSession.Token, appSession.CreateAt, appSession.IsActived }) ;
            }

            return appSession;
        }
    }
}
