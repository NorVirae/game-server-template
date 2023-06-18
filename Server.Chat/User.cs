using Server.DataAccess.Models;
using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Chat
{
    public class User : BaseRepository<UserModel>
    {
        public User(string connetionstring, Log log) : base(connetionstring, log)
        {

        }

        public async Task<int> StoreUser(UserModel chat)
        {
            var queryString = $"INSERT INTO user (id, userplayfabid, userid) Values(@id, @userplayfabid, @userid);";

            int result = await ExecuteAsync(queryString, chat);
            return result;
        }

        public async Task<List<UserModel>> FetchUsers()
        {
            var queryString = $"SELECT * FROM user";

            return await QueryAsync(queryString, null);
        }

        public async Task<UserModel> FetchUser(string userid)
        {
            var queryString = $"SELECT * FROM user WHERE id=@userid;";

            return await QuerySingleAsync(queryString, new { Id = userid });
        }

        public async Task<int> UpdateUser(UserModel chat)
        {
            var queryString = $"UPDATE user (id, userplayfabid, userid) Values(@id, @userplayfabid, @userid) WHERE id=@id";

            return await ExecuteAsync(queryString, chat);

        }

        public async Task<int> DeleteUser(int userid)
        {
            var queryString = $"DELETE FROM user WHERE id=@userid";

            return await ExecuteAsync(queryString, new { id = userid });

        }
    }
}
