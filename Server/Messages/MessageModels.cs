using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Ably.Realtime;
using Server.DataAccess.Models;

namespace Server
{
    public class SystemMessage : Message
    {
        public SystemMessageType messageType;
        public string message;
        
    }

    public class LoginMessage : Message
    {
        public string UserId;
        public string PlayfabId;
    }

    public class ChatMessage : Message
    {
        public Guid Id { get; set; }
        public string SenderPlayfabId { get; set; }
        public string ReceiverPlayfabId { get; set; }
        public string Content { get; set; }
        public Guid ChatRoomId { get; set; }
        public string MediaUrl { get; set; }
    }

    public class PlayfabMessage : Message
    {
        public string playfabId;
        public string message;
    }

    public class FriendRequestMessage: Message
    {
        public string PlayfabId { get; set; }
        public string FriendName { get; set; }
        public string FriendAvatarUrl { get; set; }
        public FriendStatus Status { get; set; }
        public bool IsSender { get; set; }
    }

    public class ChatRoomMessage : Message
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SenderPlayfabId { get; set; }
        public string ReceiverPlayfabId { get; set; }

        public List<ChatModel> Chats { get; set; }
    }
}
