using System;
using System.Collections.Generic;
using System.Text;

namespace Server.DataAccess.Utils
{
    public static class Helper
    {
        private static readonly string TagChars = "0289PYLQGRJCUV";
        private static readonly int NumTagChars = TagChars.Length;

        public static Random Random = new Random();

        public static string GetHashtagFromId(long id)
        {
            // Hashtag that we're going to return.
            var hashtag = string.Empty;

            var highInt = GetHighInt(id);
            if (highInt <= 255)
            {
                var lowInt = GetLowInt(id);

                id = (lowInt << 8) + highInt;
                while (id != 0)
                {
                    var index = id % NumTagChars;
                    hashtag = TagChars[(int)index] + hashtag;

                    id /= NumTagChars;
                }

                // Don't forget the hashtag at the end
                hashtag = "#" + hashtag;
            }

            return hashtag;
        }

        // Last 32 bits of the specified long.
        private static long GetLowInt(long l) => l & 0xFFFFFFFF;

        // First 32 bits of the specified long.
        private static long GetHighInt(long l) => l >> 32;

        public static string FirstToLowerCase(this string value)
        {
            return char.ToLower(value[0]) + value.Substring(1);
        }
        
        public static T CastTo<T>(this object value , T typeHolder)
        {
            return (T)value;
        }
        public static T GetPropertyValue<T>(this object obj, string property)
        {
            return (T)obj.GetType().GetProperty(property).GetValue(obj, null);
        }


        public static float[] GetFloatArray(Dictionary<string, Object> map, string name, float scale)
        {
            var list = (List<Object>)map[name];
            var values = new float[list.Count];
            if (scale == 1)
            {
                for (int i = 0, n = list.Count; i < n; i++)
                    values[i] = (float)list[i];
            }
            else
            {
                for (int i = 0, n = list.Count; i < n; i++)
                    values[i] = (float)list[i] * scale;
            }
            return values;
        }

        public static int[] GetIntArray(Dictionary<string, Object> map, string name)
        {
            var list = (List<Object>)map[name];
            var values = new int[list.Count];
            for (int i = 0, n = list.Count; i < n; i++)
                values[i] = (int)(float)list[i];
            return values;
        }

        public static float GetFloat(Dictionary<string, Object> map, string name, float defaultValue)
        {
            if (!map.ContainsKey(name))
                return defaultValue;
            return (float)map[name];
        }

        public static int GetInt(Dictionary<string, Object> map, string name, int defaultValue)
        {
            if (!map.ContainsKey(name))
                return defaultValue;
            return (int)(float)map[name];
        }

        public static bool GetBoolean(Dictionary<string, Object> map, string name, bool defaultValue)
        {
            if (!map.ContainsKey(name))
                return defaultValue;
            return (bool)map[name];
        }

        public static string GetString(Dictionary<string, Object> map, string name, string defaultValue)
        {
            if (!map.ContainsKey(name))
                return defaultValue;
            return (string)map[name];
        }

        public static float ToColor(string hexString, int colorIndex, int expectedLength = 8)
        {
            if (hexString.Length != expectedLength)
                throw new ArgumentException("Color hexidecimal length must be " + expectedLength + ", recieved: " + hexString, "hexString");
            return Convert.ToInt32(hexString.Substring(colorIndex * 2, 2), 16) / (float)255;
        }
    }
}
