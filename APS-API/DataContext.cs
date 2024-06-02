using Microsoft.EntityFrameworkCore;
using WebAPITEst.Entity;

namespace WebAPITEst
{
    public class DataContext : DbContext
    {

        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Writer> Writers { get; set; }
    }
}
