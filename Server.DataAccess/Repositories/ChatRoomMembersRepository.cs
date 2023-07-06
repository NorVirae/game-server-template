using Server.DataAccess.Models;
using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataAccess
{
    public class ChatRoomMembersRepository : BaseRepository<ChatRoomMembersModel> { 

            public ChatRoomMembersRepository(IDataService dataService, Log log) : base(dataService.GetConnectionString(), log) { }

        public async Task<int> StoreChatRoomMembers(ChatRoomMembersModel chatRoom)
            {
                var queryString = $"INSERT INTO chatroommembers (\"Id\", \"PlayerPlayfabId\", \"ChatRoomId\", \"UpdatedAt\", \"CreatedAt\") Values(@Id, @PlayerPlayfabId, @ChatRoomId, @UpdatedAt, @CreatedAt);";

                int result = await ExecuteAsync(queryString, chatRoom);
                Console.WriteLine(result + " INSERT WAS SUCCESSFUL!");
                return result;
            }

        public async Task<List<ChatRoomMembersModel>> FetchChatRoomMembers(Guid chatRoomMemberId)
            {
                var queryString = $"SELECT * FROM chatroommembers WHERE \"ChatRoomMemberId\"=@ChatRoomMemberId";

                return await QueryAsync(queryString, new { ChatRoomMemberId=chatRoomMemberId });
            }

        public async Task<ChatRoomMembersModel> FetchChatRoomMember(string playfabId, string chatRoomId)
            {
                var queryString = $"SELECT * FROM chatroommembers WHERE \"PlayerPlayfabId\"=@PlayerPlayfabId AND \"ChatRoomId\"=@ChatRoomId;";

                return await QuerySingleAsync(queryString, new { PlayerPlayfabId = playfabId, ChatRoomId = chatRoomId });
            }

        public async Task<List<ChatRoomMembersModel>> FetchChatRoomIdMembersWithPlayersPlayfabId(string senderPlayfabId, string receiverPlayfabId)
        {
            var queryString = $"SELECT * FROM chatroommembers WHERE \"PlayerPlayfabId\" IN (@SenderPlayfabId, @ReceiverPlayfabId);";

            return await QueryAsync(queryString, new { SenderPlayfabId = senderPlayfabId, ReceiverPlayfabId = receiverPlayfabId });
        }

        public async Task<int> UpdateChatRoomMember(ChatRoomMembersModel chatRoom)
            {
                var queryString = $"UPDATE chatroommembers (\"Id\", \"ChatRoomId\", \"PlayerPlayfabId\") Values(@Id, @ChatRoomid, @PlayerPlayfabId) WHERE \"Id\"=@Id";

                return await ExecuteAsync(queryString, chatRoom);

            }

        public async Task<int> DeleteChatRoomMembers(Guid chatRoomId, string playfabId)
            {
                var queryString = $"DELETE FROM chatroommembers WHERE ChatRoomId=@ChatRoomId AND PlayfabId=@PlayfabId;";

                return await ExecuteAsync(queryString, new { ChatRoomId = chatRoomId, PlayfabId = playfabId });

            }
    }
}
