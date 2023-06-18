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
        public Guid id { get; set; }
        public Guid senderid { get; set; }
        public Guid receiverid { get; set; }
        public string msg { get; set; }
        public Guid chatroomid { get; set; }
        
    }

    public class ChatModelInput: BaseDbModel
    {
        public Guid id { get; set; }
        public string senderid { get; set; }
        public string receiverid { get; set; }
        public string msg { get; set; }
        public Guid chatroomid { get; set; }
    }
}
