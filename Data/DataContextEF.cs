using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<User> Users { get; set; }
        
    }
}