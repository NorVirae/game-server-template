using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataAccess.Models
{
    public enum FriendStatus
    {
        Pending,
        Accepted,
        Declined,
    }
    public class Friend
    {
        public string PlayfabId { get; set; }
        public string FriendName { get; set; }
        public string FriendAvatarUrl { get; set; }
        public FriendStatus Status { get; set; }
        public bool IsSender { get; set; }

    }
}
