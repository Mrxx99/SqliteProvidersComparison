using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace MicrosoftEntitiyFrameworkCoreSqlite
{
    public class MicrosoftEntityFrameworkCoreSqliteExecuter : ISqliteExecuter
    {
        private UsersDbContext _dbContext;

        public void Initilize()
        {
            _dbContext = new UsersDbContext();
            _dbContext.Database.OpenConnection();
        }

        public void CreateTables()
        {
            _dbContext.Database.EnsureCreated();
        }

        public void InsertEntries(IEnumerable<User> users)
        {
            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();
        }

        public IEnumerable<User> ReadEntries()
        {
            return _dbContext.Users.ToList();
        }

        public void Dispose()
        {
            _dbContext.Database.CloseConnection();
            _dbContext.Dispose();
        }
    }
}
