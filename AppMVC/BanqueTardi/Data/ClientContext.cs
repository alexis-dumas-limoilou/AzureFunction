using Microsoft.EntityFrameworkCore;
using BanqueTardi.Models;
using System;
using System.Numerics;

namespace BanqueTardi.Data
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options)
          : base(options)
        {
        }

        public DbSet<TypeCompte> TypeComptes { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Banque> Banques { get; set; }

        public DbSet<Compte> Comptes { get; set; }

        public DbSet<Operation> Operations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeCompte>().ToTable("TypeComptes");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Banque>().ToTable("Banques");
            modelBuilder.Entity<Compte>().ToTable("Comptes");
            modelBuilder.Entity<Operation>().ToTable("Operations");

            modelBuilder.Entity<Compte>()
                .HasKey(c => new { c.CompteId, c.TypeCompteID });

            modelBuilder.Entity<Operation>()
                .HasOne(o => o.Compte)
                .WithMany(c => c.Operations)
                .HasForeignKey(o => new { o.CompteId, o.TypeCompteID });
          
        }
    }
}
