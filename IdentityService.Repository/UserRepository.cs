using Microsoft.Extensions.Configuration;
using IdentityService.Domain;
using System;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using IdentityService.Domain.Config;
using Microsoft.Extensions.Options;
using Npgsql;

namespace IdentityService.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly IRoleRepository roleRepository;
        private string _connectionStrings;


        public UserRepository
            (
                IRoleRepository roleRepository,
                IOptions<ConnectionStrings> config
            ) : base(config)
        {
            this.roleRepository = roleRepository;
            _connectionStrings = config.Value.IdentityServerConnection;
        }

        public async Task AddAsync(User user, string salt)
        {
            var sql = "insert into Users(id, email, password, name, salt, firstname, lastname, avatar, about, occupation, phone)values(@id, @email, @password, @name, @salt, @firstname, @lastname, @avatar, @about, @occupation, @phone)";

            using var conn = new NpgsqlConnection(_connectionStrings);
            await conn.ExecuteAsync(sql, new { user.Id, user.Email, user.Password, user.Name, salt, user.firstname, user.lastname, user.avatar, user.about, user.occupation, user.phone});
        }

        public async Task ChangePasswordAsync(Password password)
        {
            var sql = "update Users set Password=@newPassword, Salt=@salt where Email=@email and Password=@currentPassword";
            using var conn = new NpgsqlConnection(_connectionStrings);

            await conn.ExecuteAsync(sql, new
            {
                password.NewPassword,
                password.CurrentPassword,
                password.Salt,
                password.Email
            });
        }

        public async Task<bool> ExistsAsync(string email)
        {
            var sql = "select count(*) from Users where Email=@email";
            using var conn = new NpgsqlConnection(_connectionStrings);
            var res = await conn.ExecuteScalarAsync<long>(sql, new { email });

            return res > 0;
        }

        public async Task<ResetTicket> GetResetTicketAsync(Guid? resetTicket)
        {
            var sql = "SELECT ResetTicket Ticket, ResetTicketExp ExpireDate FROM Users WHERE ResetTicket = @resetTicket";
            using var conn = new NpgsqlConnection(_connectionStrings);
            var ticket = await conn.QueryFirstOrDefaultAsync<ResetTicket>(sql, new { resetTicket });

            return ticket;
        }

        public async Task<IEnumerable<UserRole>> GetRolesAsync(Guid? idUser, Guid? idAudience)
        {
            var sql = "select value from UserRoles where IdUser = @idUser and IdAudience = @idAudience";
            using var conn = new NpgsqlConnection(_connectionStrings);
            var roles = await conn.QueryAsync<UserRole>(sql, new { idUser, idAudience });

            return roles;
        }

        public async Task<IEnumerable<UserEdit>> ListAsync()
        {
            using var conn = new NpgsqlConnection(_connectionStrings);


            var editUsers = await conn.QueryAsync<UserEdit>("select * from Users where Email <> 'admin' order by Name");

            var userRoles = await conn.QueryAsync<UserRole>($@"select ur.IdUser, ur.IdAudience, ur.IdRole, ur.Value, r.Name
                                                                     from UserRoles ur
                                                                     inner join Roles r on r.Id = ur.IdRole where ur.IdUser = ANY(@users)",
                    new { users = editUsers.Select(x => x.Id.Value).ToArray() }
            );

            var audiences = await conn.QueryAsync<Audience>("select Id, Name from Audiences where Id = ANY(@audiences)",
                    new { audiences = userRoles.Select(x => x.IdAudience).Distinct().ToArray() }
            );

            foreach (var user in editUsers)
            {
                var userAudiences = userRoles.Where(x => x.IdUser == user.Id).Select(x => x.IdAudience).Distinct();
                user.Audiences.AddRange(userAudiences.Select(x => new Audience { Id = x }));

                foreach (var audience in user.Audiences)
                {
                    var roles = userRoles.Where(x => x.IdUser == user.Id && x.IdAudience == audience.Id);

                    audience.Roles.AddRange(roles);
                    audience.Name = audiences.FirstOrDefault(x => x.Id == audience.Id).Name;
                }
            }

            return editUsers;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var sql = $"select * from users where email like '{email}%'";
            using var conn = new NpgsqlConnection(_connectionStrings);
            return await conn.QueryFirstOrDefaultAsync<User>(sql);
        }

        public async Task<User> GetUserByUserIdAsync(string userId)
        {
            var sql = $"select * from users where id ='{userId}'";
            using var conn = new NpgsqlConnection(_connectionStrings);
            return await conn.QueryFirstOrDefaultAsync<User>(sql);
        }

        public async Task NotifyResetPasswordAsync(string email, Guid resetTicket, DateTime resetTicketExp)
        {
            var sql = "update users set ResetTicket=@resetTicket, ResetTicketExp=@resetTicketExp where Email=@email";
            using var conn = new NpgsqlConnection(_connectionStrings);
            await conn.ExecuteAsync(sql, new { email, resetTicket, resetTicketExp });
        }

        public async Task RemoveResetTicketAsync(Guid resetTicket)
        {
            var sql = "update Users set ResetTicket = null, ResetTicketExp = null where ResetTicket = @resetTicket";
            using var conn = new NpgsqlConnection(_connectionStrings);
            await conn.ExecuteAsync(sql, new { resetTicket });
        }

        public async Task<bool> ResetPasswordAsync(Password password, Guid? resetTicket)
        {
            var sql = "update users set Password=@newPassword, Salt=@salt, ResetTicket=null, ResetTicketExp=null where ResetTicket=@resetTicket and ResetTicketExp > NOW()";
            using var conn = new NpgsqlConnection(_connectionStrings);
            var res = await conn.ExecuteAsync(sql, new { password.NewPassword, password.Salt, resetTicket });
            return res == 1;
        }

        public async Task UpdateAsync(UserEdit userEdit)
        {
            var userRoles = userEdit.Audiences.SelectMany(x => x.Roles).ToList();
            var ids = userRoles.Select(x => x.IdRole);
            var roles = await roleRepository.ListAsync(ids);
            userRoles.ForEach(x => x.Value = roles.FirstOrDefault(r => r.Id == x.IdRole)?.Value);

            var sql = "delete from userRoles where IdUser = @IdUser";

            using var conn = new NpgsqlConnection(_connectionStrings);
            await conn.ExecuteAsync(sql, new { idUser = userEdit.Id });

            sql = "insert into userRoles(IdRole, IdUser, IdAudience, Value)values(@idRole, @idUser, @idAudience, @value)";
            await conn.ExecuteAsync(sql, userRoles);

        }

        public async Task AddRole(Guid idRole, Guid idUser, Guid idAudience, string value)
        {
            var sql = "insert into userRoles(IdRole, IdUser, IdAudience, Value)values(@idRole, @idUser, @idAudience, @value)";
            using var conn = new NpgsqlConnection(_connectionStrings);
            await conn.ExecuteAsync(sql, new { idRole, idUser, idAudience, value });
        }

        public async Task<bool> ResetPasswordAsync(Password password)
        {
            var sql = "update users set Password=@newPassword, Salt=@salt where email=@email";
            using var conn = new NpgsqlConnection(_connectionStrings);
            var res = await conn.ExecuteAsync(sql, new { salt = password.Salt, newPassword = password.NewPassword, email = password.Email });
            return res == 1;
        }

    }
}
