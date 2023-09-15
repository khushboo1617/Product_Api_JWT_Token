using Microsoft.EntityFrameworkCore;
using Product_Api_JWT_Token.Models;

namespace Product_Api_JWT_Token.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Product> products { get; set; }
    }
}
