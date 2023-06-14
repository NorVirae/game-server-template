using CyberspawnsServer.DataAccess.Models;
using CyberSpawnsServer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnsServer.Chat
{
    public class Chat : BaseRepository<ChatModel>
    {
        public Chat(string connetionstring, Log log) : base(connetionstring, log)
        {

        }

        public async Task<int> StoreChat(ChatModel chat)
        {
            var queryString = $"INSERT INTO Chat (Id, UserId, Message, ChatRoomId) Values($Id, $UserId, $Message, $ChatRoomId) WHERE Id=$Id;";
            return await ExecuteAsync(queryString, chat);
        }

        public async Task<List<ChatModel>> FetchChatHistory()
        {
            var queryString = $"SELECT * FROM Chat";

            return await QueryAsync(queryString, null);
        }

        public async Task<ChatModel> FetchChat(int chatId)
        {
            var queryString = $"SELECT * FROM Chat WHERE Id=$Id;";

            return await QuerySingleAsync(queryString, new {Id=chatId});
        }

        public async Task<ChatModel> UpdateChat(ChatModel chat)
        {
            var queryString = $"UPDATE Chat (Id, UserId, Message, ChatRoomId) Values($Id, $UserId, $Message, $ChatRoomId) WHERE Id=$Id";

            return await QuerySingleAsync(queryString, chat);
        }
    }
}
