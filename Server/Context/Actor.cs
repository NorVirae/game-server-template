using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Context
{
    public abstract class Actor<T> : IActor
    {
        public static T Instance { get; protected set; }
    }
}
