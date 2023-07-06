using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public interface ISession
    {
        double CreatedAt { get; set; }
        double UpdatedAt { get; set; }
        void Save();
    }
}
