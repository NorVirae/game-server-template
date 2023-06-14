using CyberspawnsServer.DataAccess.Models;
using CyberSpawnsServer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CyberspawnsServer.Chat
{
    public class Chat : BaseRepository<Chat>
    {
        public Chat(string connetionstring, Log log) : base(connetionstring, log)
        {

        }

        public async Task<Chat> StoreChat(Chat chat)
        {
            return 
        }

        public async Task<Chat> FetchChatHistory(int chatRoomid)
        {

        }

        public async Task<Chat> FetchChat(int chatId)
        {

        }
    }
}
