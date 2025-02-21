using CarteDeCredit.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CarteDeCredit.API.Data
{
    public class CarteDeCreditDBContext : DbContext
    {
        public CarteDeCreditDBContext(DbContextOptions<CarteDeCreditDBContext> options)
          : base(options)
        {
        }

        public DbSet<CarteCredit> CarteCredits { get; set; }
    }
}
