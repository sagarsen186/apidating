//using Microsoft.EntityFramwork
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Register.Model
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }  
    }
}
