namespace Server.Core
{
    public class Singliton<T> where T : class
    {
        public static T Instance { get; protected set; }

        
    }
}