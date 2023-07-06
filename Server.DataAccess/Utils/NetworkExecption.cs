using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class InvalidEventException : Exception
    {
        public string eventType;
        public InvalidEventException() { }
        public InvalidEventException(string message) : base(message){  }

        public InvalidEventException(string message, Exception inner) : base(message, inner){ }

        public InvalidEventException(string message, string type) : base(message)
        {
            eventType = type;
        }
    }

    public class NetworkException : Exception
    {
        public NetworkException() { }
        public NetworkException(string message) : base(message) { }

        public NetworkException(string message, Exception inner) : base(message, inner) { }
    }

    public class DataOperationExeption : Exception
    {
        public DataOperationExeption() {  }
        public DataOperationExeption(string message) : base(message) { 

        }
    }
}
