using Server.Core;


namespace Server
{
    public abstract class Message
    {
        public string ToJSon()
        {
            return SerializationHelper.Serialize(this);
        }
    }

    public class MessageProxy
    {
        public short messageID;
        public object messageBody;
    } 
}
