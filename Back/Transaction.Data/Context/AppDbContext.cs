using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction.Core.Entities;


namespace Transaction.Data.Context
{
    public class AppDbContext : DbContext
    {
        // Constructeur avec injection des options
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
      public DbSet <User> Users { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }
        public DbSet<Transact> Transactions { get; set; }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Appel des méthodes de configuration
            ConfigureUser(modelBuilder);
            ConfigureAuthToken(modelBuilder);
            ConfigureTransact(modelBuilder);
            ConfigureItem(modelBuilder);

            // Insertion de données initiales
            SeedData(modelBuilder);
        }
        private void ConfigureUser(ModelBuilder modelBuilder)
        {
           var entity= modelBuilder.Entity<User>();

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(u => u.PasswordHash)
                  .IsRequired();

            // Relation 1 à N avec AuthTokens
            entity.HasMany(u => u.AuthTokens)
                  .WithOne(at => at.User)
                  .HasForeignKey(at => at.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relation 1 à N avec Transactions
            entity.HasMany(u => u.Transactions)
                  .WithOne(t => t.User)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

        }
        private void ConfigureAuthToken(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<AuthToken>();

            entity.HasKey(u => u.Id);

            entity.Property(u => u.EmissionDate)
                  .IsRequired() ;
            entity.Property(u => u.ExpirationDate)
                 .IsRequired();
            entity.HasOne(u => u.User)
                 .WithMany(at => at.AuthTokens)
                 .HasForeignKey(at => at.UserId);
        }
        private void ConfigureTransact(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Transact>();

            entity.HasKey(u => u.Id);

            entity.Property(u => u.TransactionDate)
                  .IsRequired();
            entity.HasOne(u => u.User)
                 .WithMany(at => at.Transactions)
                 .HasForeignKey(at => at.UserId);

            entity.HasMany(u => u.Items)
                 .WithMany(at => at.Transactions);
        }
        private void ConfigureItem(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Item>();
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id);
            entity.Property(u => u.Name).IsRequired();

            entity.Property(u => u.Description);

            entity.Property(u => u.Price).IsRequired();

            entity.HasMany(u => u.Transactions)
                 .WithMany(at => at.Items);

        }
        private void SeedData(ModelBuilder modelBuilder)
        {

            ///<summary>
            ///inserer les user par défauts
            ///</summary>

            modelBuilder.Entity<User>().HasData(

                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@example.com",
                    PasswordHash = "password123"
                }
                );
            // Insérer des items par défaut
            modelBuilder.Entity<Item>().HasData(
                 new Item
                 {
                     Id = 1,
                     Name = "Épée de Feu",
                     Description = "Une épée légendaire qui inflige des dégâts de feu.",
                     Price = 150.0f
                 },
                    new Item
                    {
                        Id = 2,
                        Name = "Potion de Soin",
                        Description = "Restaure 50 points de vie instantanément.",
                        Price = 25.0f
                    },
                    new Item
                    {
                        Id = 3,
                        Name = "Bouclier du Dragon",
                        Description = "Augmente la défense de 20 points.",
                        Price = 200.0f
                    }
            );
            


        }

        }
}
