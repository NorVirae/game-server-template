using CyberSpawnsServer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnsServer.DataAccess.Models
{
    public class Chat: BaseDbModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string message { get; set; }
        public Guid ChatRoomId { get; set; }
    }
}
