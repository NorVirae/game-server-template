using Server.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.DataAccess
{
    public interface IDataService
    {
        string GetConnectionString();
        void Log(object message);

        UserRepository User { get; }
    }
}
