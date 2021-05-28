using CreditCardGenerator.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditCardGenerator.Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext()
        {
        }
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }
        public DbSet<CreditCard> CreditCards { get; set; }
    }
}
