using Server.DataAccess.Models;
using Server.DataAccess.Utils.DB;
using Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.DataAccess.Utils;

namespace Server.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<UserModel>
    {
        private readonly IDataService dataService;
        public UserRepository(IDataService dataService) : base(dataService.GetConnectionString(), dataService.Log)
        {
            this.dataService = dataService;
        }
        public async Task<UserModel> GetUserAsync(string id)
        {
            if (id == null)
                return null;

            string query = DbHelper.SelectAllFrom(table) +
                DbHelper.Where("id", "Id") + DbHelper.END;
            UserModel user = await QueryFirstOrDefaultAsync(query, new { Id = id });
            return user;
        }

        
    }
}
