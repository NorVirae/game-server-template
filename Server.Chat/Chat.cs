using Server.DataAccess.Models;
using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Chat
{
    public class Chats : BaseRepository<ChatModel>
    {
        public Chats(string connetionstring, Log log) : base(connetionstring, log)
        {

        }

        public async Task<int> StoreChat(ChatModel chat)
        {
            var queryString = $"INSERT INTO chat (id, senderid, receiverid, msg, chatroomid) Values(@id, @senderid, @receiverid, @msg, @chatroomid);";

            int result = await ExecuteAsync(queryString, chat);
            Console.WriteLine(queryString + " RESULT " + chat.id + " id " + chat.senderid + " senderid " + chat.receiverid + " receiverid " + chat.chatroomid + " chatroomid " + chat.msg + " msg ");
            Console.WriteLine("RESULT " + result);
            return result;
        }

        public async Task<List<ChatModel>> FetchChatHistory()
        {
            var queryString = $"SELECT * FROM Chat";

            return await QueryAsync(queryString, null);
        }

        public async Task<ChatModel> FetchChat(int chatId)
        {
            var queryString = $"SELECT * FROM Chat WHERE Id=@Id;";

            return await QuerySingleAsync(queryString, new {Id=chatId});
        }

        public async Task<int> UpdateChat(ChatModel chat)
        {
            var queryString = $"UPDATE Chat (Id, UserId, Message, ChatRoomId) Values(@Id, @UserId, @Message, @ChatRoomId) WHERE Id=@Id";

            return await ExecuteAsync(queryString, chat);

        }

        public async Task<int> DeleteChat(int id)
        {
            var queryString = $"DELETE FROM Chat WHERE Id=@Id";

            return await ExecuteAsync(queryString, new {id = id});

        }
    }
}
