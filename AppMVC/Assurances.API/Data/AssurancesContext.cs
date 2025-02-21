using Assurances.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Assurances.API.Data
{
    public class AssurancesContext : DbContext
    {
        public AssurancesContext(DbContextOptions<AssurancesContext> options)
          : base(options)
        {
        }

        public DbSet<ContratAssurance> ContratAssurances { get; set; }
    }
}
