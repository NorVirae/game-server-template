using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QNetLib;

namespace CyberspawnsServer
{
    public class AppThread
    {
        public static AppThread instance;
        private Thread thread;
        private volatile bool running;
        private RingBuffer<Action> callBackPool;

        public AppThread()
        {
            instance = this;
            callBackPool = new RingBuffer<Action>(256);
        }

        public void Start(ThreadStart threadStart)
        {

            thread = new Thread( () => {
                running = true;
                while (running)
                {
                    Thread.Sleep(2);
                    PoolSchedule();
                    threadStart.Invoke();
                    
                }
                    
            });
            thread.IsBackground = true;
            thread.Name = "AppThread";
            thread.Start();
        }


        public static void Schedule(Action callback)
        {
            instance.callBackPool.Enqueue(callback);
        }

        public void PoolSchedule()
        {
            if(callBackPool.Count > 0)
            {
                callBackPool.Dequeue().Invoke();
            }
        }

        public static void Schedule(Action callback, float delay)
        {
            Task.Delay((int)(delay * 1000)).ContinueWith(t => callback());
        }
        

        public void Stop()
        {
            if (running)
            {
                running = false;
                thread.Join();
            }
        }
    }
}
