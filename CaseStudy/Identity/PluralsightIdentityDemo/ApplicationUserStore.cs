using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace PluralsightIdentityDemo
{
   public class ApplicationUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
   {
      #region UserStore implementation

      public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         using (var connection = GetOpenConnection())
         {
            var rowCount = await connection.ExecuteAsync(
               "INSERT INTO [ApplicationUsers]([Id]," +
               "[UserName]," +
               "[NormalizedUserName]," +
               "[PasswordHash]) " +
               "VALUES " +
               "(@id,@userName,@normalizedUserName,@passwordHash)",
               new
               {
                  id = user.Id,
                  userName = user.UserName,
                  normalizedUserName = user.NormalizedUserName,
                  passwordHash = user.PasswordHash
               });
            if (rowCount == 1)
               return IdentityResult.Success;
            else
               return IdentityResult.Failed();
         }
      }

      public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         using (var connection = GetOpenConnection())
         {
            var rowCount = await connection.ExecuteAsync("DELETE FROM [ApplicationUsers] WHERE [Id] = @id", new { id = user.Id });
            if (rowCount == 1)
               return IdentityResult.Success;
            else
               return IdentityResult.Failed();
         }

      }

      public void Dispose()
      {
      }

      public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
      {
         using (var connection = GetOpenConnection())
         {
            return await connection.QueryFirstOrDefaultAsync(
               "SELECT * FROM [ApplicationUsers] WHERE Id = @id",
               new { id = userId });
         }
      }

      public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
      {
         using (var connection = GetOpenConnection())
         {
            var user = await connection.QueryFirstOrDefaultAsync<ApplicationUser>(
               "SELECT * FROM [ApplicationUsers] WHERE NormalizedUserName = @name",
               new { name = normalizedUserName });

            return user;
         }
      }

      public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         return Task.FromResult(user.NormalizedUserName);
      }

      public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         return Task.FromResult(user.Id);
      }

      public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         return Task.FromResult(user.UserName);
      }

      public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
      {
         user.NormalizedUserName = normalizedName;
         return Task.CompletedTask;
      }

      public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
      {
         user.UserName = userName;
         return Task.CompletedTask;
      }

      public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         using (var connection = GetOpenConnection())
         {
            var rowCount = await connection.ExecuteAsync(
               "UPDATE [ApplicationUsers] " +
               "SET [UserName] = @userName, " +
               "[NormalizedUserName] = @normalizedUserName, " +
               "[PasswordHash] = @passwordHash) " +
               "WHERE [id] = @id",
               new
               {
                  id = user.Id,
                  userName = user.UserName,
                  normalizedUserName = user.NormalizedUserName,
                  passwordHash = user.PasswordHash
               });
            if (rowCount == 1)
               return IdentityResult.Success;
            else
               return IdentityResult.Failed();
         }
      }

      #endregion

      #region PasswordUserStore implementation

      public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
      {
         user.PasswordHash = passwordHash;
         return Task.CompletedTask;
      }

      public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         return Task.FromResult(user.PasswordHash);
      }

      public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
      {
         return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
      }

      #endregion

      #region Database Access

      private static DbConnection GetOpenConnection()
      {
         var connection = new SqlConnection("Server=TRUNG-LUU\\TRUNGSQLSERVER;Database=PluralsightDemo;User ID=sa;Password=Trung1997;");
         // var connection = new SqlConnection("Server=localhost,1433;Database=PluralsightDemo;User ID=sa;Password=Trung1997;");

         connection.Open();

         return connection;
      }

      #endregion
   }
}
