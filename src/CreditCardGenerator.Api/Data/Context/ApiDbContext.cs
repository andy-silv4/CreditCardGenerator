using CreditCardGenerator.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditCardGeneratorAPI.Data
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
