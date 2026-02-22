using Microsoft.EntityFrameworkCore;
using PublicServiceRequestBackend.Models;

namespace PublicServiceRequestBackend.Data
{
    public class PublicServiceRequestDbContext : DbContext 
    {
        public PublicServiceRequestDbContext(DbContextOptions<PublicServiceRequestDbContext> options) : base(options)
        {
        }

        public DbSet<ServiceRecordEntity> ServiceRecords { get; set; }
    }
}
