using Microsoft.EntityFrameworkCore;

namespace Server.Data
{
    public class DatabaseService : IDatabaseService
    {
        private readonly DataContext _context;
        public DatabaseService(DataContext context)
        {
            _context = context;
        }
        public void InitializeDatabase()
        {
            _context.Database.ExecuteSqlCommand(@"CREATE TABLE IF NOT EXISTS Values (id SERIAL PRIMARY KEY, Number INT)");
            _context.SaveChanges();        
        }
    }
}