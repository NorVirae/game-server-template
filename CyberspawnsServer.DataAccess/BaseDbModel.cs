
using CyberSpawnsServer.DataAccess.Attributes;
using System;
using System.Reflection;


namespace CyberSpawnsServer.DataAccess
{
    public class BaseDbModel
    {
        public double CreatedAt { get; set; }
        public double UpdatedAt { get; set; }
    }


    public class DataUtils
    {
        //public static object TableAttriblute { get; private set; }

        /// <summary>
        /// Get the model table name
        /// </summary>
        /// <typeparam name="T">class type</typeparam>
        /// <returns></returns>
        public static string GetTableName<T>()
        {
            Type type = typeof(T);
            TableAttribute a = type.GetCustomAttribute<TableAttribute>(false);
            if (a == null) return "";
            return a.table;
        }

        /// <summary>
        /// Get the model memeber name tied to the tble member attribute
        /// </summary>
        /// <typeparam name="T">class type</typeparam>
        /// <param name="member">property member name</param>
        /// <returns></returns>
        public static string GetTableMemberName<T>(string member)
        {
            Type type = typeof(T);
            TableMemberAttribute a = type.GetProperty(member).GetCustomAttribute<TableMemberAttribute>();
            if (a == null) return "";
            return a.memberName;
        }
    }
}
