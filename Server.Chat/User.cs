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
            var queryString = $"INSERT INTO users (id, playfabuserid, playfabid) Values(@id, @playfabuserid, @playfabid);";

            int result = await ExecuteAsync(queryString, chat);
            return result;
        }

        public async Task<List<UserModel>> FetchUsers()
        {
            var queryString = $"SELECT * FROM users";

            return await QueryAsync(queryString, null);
        }

        public async Task<UserModel> FetchUser(string userid)
        {
            Console.WriteLine(userid + " User ID");
            var queryString = $"SELECT * FROM users WHERE playfabuserid=@Id OR playfabid=@Id;";

            return await QuerySingleAsync(queryString, new { Id = userid });
        }

        public async Task<int> UpdateUser(UserModel chat)
        {
            var queryString = $"UPDATE users (id, userplayfabid, userid) Values(@id, @userplayfabid, @userid) WHERE id=@id";

            return await ExecuteAsync(queryString, chat);

        }

        public async Task<int> DeleteUser(int userid)
        {
            var queryString = $"DELETE FROM users WHERE id=@userid";

            return await ExecuteAsync(queryString, new { id = userid });

        }
    }
}
