using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataAccess.Models
{
    public class UserModel : BaseDbModel
    {
        /// <summary>
        /// Identification of the user in the whole system
        /// </summary>
        public string id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public bool isActive { get; set; }
        public string image { get; set; }
        public string playfabuserid { get; set; }
        public bool isVerified { get; set; }

    }
}
