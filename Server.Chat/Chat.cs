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
        public Chats(string connetionstring, Log log) : base(connetionstring, log){}

        public async Task<int> StoreChat(ChatModel chat)
        {
            var queryString = $"INSERT INTO chat (id, senderid, receiverid, msg, chatroomid) Values(@id, @senderid, @receiverid, @msg, @chatroomid);";

            int result = await ExecuteAsync(queryString, chat);
            Console.WriteLine(queryString + " RESULT " + chat.id + " id " + chat.senderid + " senderid " + chat.receiverid + " receiverid " + chat.chatroomid + " chatroomid " + chat.msg + " msg ");
            Console.WriteLine("RESULT " + result);
            return result;
        }

        public async Task<List<ChatModel>> FetchChatHistory(Guid chatrmid)
        {
            var queryString = $"SELECT * FROM chat WHERE chatroomid=@chatroomid";

            return await QueryAsync(queryString, new {chatroomid = chatrmid});
        }

        public async Task<ChatModel> FetchChat(int chatId)
        {
            var queryString = $"SELECT * FROM chat WHERE id=@id;";

            return await QuerySingleAsync(queryString, new {Id=chatId});
        }

        public async Task<int> UpdateChat(ChatModel chat)
        {
            var queryString = $"UPDATE chat (id, userid, msg, chatroomid) Values(@id, @userid, @msg, @chatroomid) WHERE id=@id";

            return await ExecuteAsync(queryString, chat);

        }

        public async Task<int> DeleteChat(int id)
        {
            var queryString = $"DELETE FROM chat WHERE id=@id";

            return await ExecuteAsync(queryString, new {id = id});

        }
    }
}
