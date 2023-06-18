using Server.DataAccess.Models;
using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Chat
{
    public class ChatRoom : BaseRepository<ChatRoomModel> { 

            public ChatRoom(string connetionstring, Log log) : base(connetionstring, log){}

            public async Task<int> StoreChatRoom(ChatRoomModel chatRoom)
            {
                var queryString = $"INSERT INTO chatroom (id, title, creatorid) Values(@id, @title, @creatorid);";

                int result = await ExecuteAsync(queryString, chatRoom);
                Console.WriteLine("RESULT " + result);
                return result;
            }

            public async Task<List<ChatRoomModel>> FetchChatRoomHistory(string userid)
            {
                var queryString = $"SELECT * FROM chatroom WHERE=@userid";

                return await QueryAsync(queryString, new {userid});
            }

            public async Task<ChatRoomModel> FetchChatRoom(Guid chatRoomId)
            {
                var queryString = $"SELECT * FROM chatroom WHERE id=@Id;";

                return await QuerySingleAsync(queryString, new { Id = chatRoomId });
            }

            public async Task<int> UpdateChatRoom(ChatRoomModel chatRoom)
            {
                var queryString = $"UPDATE chatroom (id, title, topic, description, creatorid) Values(@id, @title, @topic, @description, @creatorid) WHERE id=@id";

                return await ExecuteAsync(queryString, chatRoom);

            }

            public async Task<int> DeleteChatRoom(int chatRoomId)
            {
                var queryString = $"DELETE FROM chatroom WHERE id=@Id";

                return await ExecuteAsync(queryString, new { id = chatRoomId });

            }
    }
}
