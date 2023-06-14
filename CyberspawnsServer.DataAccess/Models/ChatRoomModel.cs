using CyberSpawnsServer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnsServer.DataAccess.Models
{
    public class ChatRoom: BaseDbModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }

    }
}
