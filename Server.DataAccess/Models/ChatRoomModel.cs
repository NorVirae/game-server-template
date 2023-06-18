using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataAccess.Models
{
    public class ChatRoomModel: BaseDbModel
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string topic { get; set; }
        public string description { get; set; }
        public Guid creatorid { get; set; }

    }
}
