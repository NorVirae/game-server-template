using System;
using System.Collections.Generic;
using System.Text;

namespace Server.DataAccess.Utils.DB
{
    public class DbBuilder
    {

        private StringBuilder stringBuilder;

        public DbBuilder()
        {
            stringBuilder = new StringBuilder();
        }

        public DbBuilder SelectAllFrom(string database)
        {
            stringBuilder.Append(DbHelper.SelectAllFrom(database));
            return this;
        }

        public DbBuilder Where(string coulumn, string target)
        {
            stringBuilder.Append(DbHelper.Where(coulumn, target));
            return this;
        }

        public DbBuilder End()
        {
            stringBuilder.Append(";");
            return this;
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }
}
