using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataAccess.Models
{
    public class ChatModel: BaseDbModel
    {
        public Guid Id { get; set; }
        public string SenderPlayfabId { get; set; }
        public string ReceiverPlayfabId { get; set; }
        public string Content { get; set; }
        public Guid ChatRoomId { get; set; }
        public string MediaUrl { get; set; }
        
    }
}
