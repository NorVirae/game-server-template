using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSpawnsServer.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string table;
        public TableAttribute(string tableName)
        {
            this.table = tableName;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class TableMemberAttribute : Attribute
    {
        public string memberName;
        public TableMemberAttribute(string memberName)
        {
            this.memberName = memberName;
        }
    }
}
