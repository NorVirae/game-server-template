
using Newtonsoft.Json;

namespace Server
{
    public interface IEvent
    {
        EventType type { get; }
    }

    public struct LoginEvent : IEvent
    {
        [JsonIgnore]public EventType type => EventType.Login;
        public string token;
        public string userId;
        public string playfabId;
    }

    public struct PongEvent : IEvent
    {
        [JsonIgnore] public EventType type => EventType.Pong;
        public double timeStamp;
    }


    public struct MessageEvent : IEvent
    {
        [JsonIgnore] public EventType type => EventType.Message;
        public object body;
        public object id;
    }
}
