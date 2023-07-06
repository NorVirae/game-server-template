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
        public Guid Id { get; set; }
        public string Name { get; set; }

    }


    public class ChatRoomMembersModel : BaseDbModel
    {
        public Guid Id { get; set; }
        public Guid ChatRoomId { get; set; }
        public string PlayerPlayfabId { get; set; }

    }
}
