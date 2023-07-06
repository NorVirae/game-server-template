using Server.DataAccess.Models;
using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataAccess
{
    public class ChatRepository : BaseRepository<ChatModel>
    {
        public ChatRepository(IDataService dataService, Log log) : base(dataService.GetConnectionString(), log){}

        public async Task<int> StoreChat(ChatModel chat)
        {
            var queryString = $"INSERT INTO chats (\"Id\", \"SenderPlayfabId\", \"ReceiverPlayfabId\", \"Content\", \"ChatRoomId\", \"MediaUrl\", \"UpdatedAt\", \"CreatedAt\") Values(@Id, @SenderPlayfabId, @ReceiverPlayfabId, @Content, @ChatRoomId, @MediaUrl,  @UpdatedAt, @CreatedAt);";

            int result = await ExecuteAsync(queryString, chat);
            return result;
        }

        public async Task<List<ChatModel>> FetchChatHistory(Guid chatrmid)
        {
            var queryString = $"SELECT * FROM chats WHERE \"ChatRoomId\"=@ChatRoomId";

            return await QueryAsync(queryString, new {ChatRoomId = chatrmid});
        }

        public async Task<ChatModel> FetchChat(int chatId)
        {
            var queryString = $"SELECT * FROM chats WHERE Id=@id;";

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
