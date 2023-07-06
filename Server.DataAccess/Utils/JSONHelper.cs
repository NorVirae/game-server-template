using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Server
{
    public class JSONHelper
    {
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
