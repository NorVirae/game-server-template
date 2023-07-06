using Server.DataAccess.Models;
using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataAccess
{
    public class ChatRoomRepository : BaseRepository<ChatRoomModel> { 

            public ChatRoomRepository(IDataService dataService, Log log) : base(dataService.GetConnectionString(), log) { }

        public async Task<int> StoreChatRoom(ChatRoomModel chatRoom)
            {
                var queryString = $"INSERT INTO chatroom (\"Id\", \"Name\", \"UpdatedAt\", \"CreatedAt\") VALUES (@Id, @Name, @UpdatedAt, @CreatedAt);";
                Console.WriteLine("Name " + chatRoom.Name + " Id " + chatRoom.Id  + " Update " + chatRoom.UpdatedAt + " Create " + chatRoom.CreatedAt);
                int result = await ExecuteAsync(queryString, chatRoom);
                Console.WriteLine(result + " INSERT WAS SUCCESSFUL!");
                return result;
            }

        public async Task<List<ChatRoomModel>> FetchChatRoomHistory(Guid chatRoomId)
            {
                var queryString = $"SELECT * FROM chatroom WHERE \"ChatRoomId\"=@ChatRoomId";

                return await QueryAsync(queryString, new {ChatRoomId= chatRoomId });
            }

        public async Task<ChatRoomModel> FetchChatRoom(Guid chatRoomId)
            {
            var queryString = $"SELECT * FROM chatroom WHERE \"Id\"=@ChatRoomId";

            return await QuerySingleAsync(queryString, new { ChatRoomId = chatRoomId });
            }

        public async Task<ChatRoomModel> FetchChatRoomWithPlayersPlayfabId(string senderPlayfabId, string receiverPlayfabId)
        {
            var queryString = $"SELECT * FROM chatroom WHERE EXISTS (SELECT 1 FROM STRING_SPLIT(MembersPlayfabId, ',') AS SplitValues WHERE SplitValues.value IN ('@Value1', '@Value2')";

            return await QuerySingleAsync(queryString, new { Value1 = senderPlayfabId, Value2= receiverPlayfabId });
        }

        public async Task<int> UpdateChatRoom(ChatRoomModel chatRoom)
            {
                var queryString = $"UPDATE chatroom (id, title, topic, description, creatorid) Values(@id, @title, @topic, @description, @creatorid) WHERE id=@id";

                return await ExecuteAsync(queryString, chatRoom);

            }

            public async Task<int> DeleteChatRoom(int chatRoomId)
            {
                var queryString = $"DELETE FROM chatroom WHERE Id=@Id";

                return await ExecuteAsync(queryString, new { id = chatRoomId });

            }
    }
}
