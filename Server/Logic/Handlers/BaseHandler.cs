using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public abstract class BaseHandler<T> where T : ISession
    {
        public T Session;
        public BaseHandler(T playerSession)
        {
            this.Session = playerSession;
            
        }

        protected abstract Task LoadDataFromDB();
        
    }
}
