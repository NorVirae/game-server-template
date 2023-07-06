using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Server.DataAccess.Utils;

namespace Server.DataAccess.Utils
{
    public static class DbHelper
    {
        public const string END = ";";
        public const string COMMA = ",";
        public const string ReturnAll = " returning *";

        public static string ReturnValue(string value) => " returning " + value;

        public static string DbFromType(Type type)
        {
            return type.Name.ToLower();
        }

        public static string InsertInto(string db) => $"INSERT INTO {db} ";


        public static string Where(string coulumn, string target)
        {
            return $" WHERE {coulumn} = @{target}";
        }

        public static string WhereIn(string coulumn, string list)
        {
            return $"WHERE {coulumn} IN @{list}";
        }

        public static string And(string coulumn, string target)
        {
            return $" AND {coulumn} = @{target}";
        }

        public static string DeleteFrom(string database)
        {
            return $"DELETE FROM {database} ";
        }

        public static string BuildPager()
        {
            string result = "OFFSET     @offset ROWS " +
                            "FETCH NEXT @Next   ROWS ONLY";
            return result;
        }

        public static string Set(string coulumn, string value)
        {
            return $" SET {coulumn} = @{value} ";
        }

        public static string AddSet(string coulumn, string value)
        {
            return $", {coulumn} = @{value} ";
        }

        public static string Update(string table)
        {
            return $" UPDATE {table} ";
        }

        public static string SelectFrom(string dataBase, params string[] targets)
        {
            StringBuilder builder = new StringBuilder("SELECT ");
            for (int i = 0; i < targets.Length - 1; i++)
            {
                builder.Append($"{targets[i]}, ");
            }
            builder.Append($"{targets[targets.Length - 1]} ");
            builder.Append($"FROM {dataBase}");
            return builder.ToString();
        }

        public static string SelectAllFrom(string dataBase)
        {
            return $"SELECT * FROM {dataBase}";
        }

        public static string BuildColumns(params string[] column)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            for (int i = 0; i < column.Length - 1; i++)
            {
                stringBuilder.Append(column[i] + ", ");
            }
            stringBuilder.Append(column[column.Length - 1] + ")");
            return stringBuilder.ToString();
        }

        public static string BuildColumns(object column)
        {
            PropertyInfo[] infos = column.GetType().GetProperties();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            for (int i = 0; i < infos.Length - 1; i++)
            {
                stringBuilder.Append(infos[i].Name.FirstToLowerCase() + ", ");
            }
            stringBuilder.Append(infos[infos.Length - 1].Name.FirstToLowerCase() + ")");
            return stringBuilder.ToString();
        }

        public static string BuildValues(params string[] column)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" VALUES (");
            for (int i = 0; i < column.Length - 1; i++)
            {
                stringBuilder.Append($" @{column[i]}, ");
            }
            stringBuilder.Append($" @{column[column.Length - 1]}) ");
            return stringBuilder.ToString();
        }

        public static string BuildValues(object data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" VALUES (");

            PropertyInfo[] infos = data.GetType().GetProperties();
            for (int i = 0; i < infos.Length - 1; i++)
            {
                stringBuilder.Append($" @{infos[i].Name}, ");
            }
            stringBuilder.Append($" @{infos[infos.Length - 1].Name}) ");
            return stringBuilder.ToString();
        }
    }
}
