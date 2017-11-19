using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using Models.Entities;
using Models.Migrations;

namespace Models
{
    public class LrdrContext : DbContext, IDisposable
    {
        public LrdrContext()
            : base($"name=App")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LrdrContext, Configuration>());
            this.Configuration.ProxyCreationEnabled = false;
        }

        public LrdrContext(string connectionStringName)
            : base(connectionStringName)
        {
        }


        public LrdrContext(string connectionStringName, bool dropDatabase)
            : base($"name={connectionStringName}")
        {
            if (dropDatabase)
            {
                Database.SetInitializer(new DropCreateDatabaseAlways<LrdrContext>());
            }
            else
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<LrdrContext, Configuration>());
            }
        }

        public new void Dispose()
        {
            base.Dispose();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.OrdersAsCustomer)
                .WithRequired()
                .HasForeignKey(u => u.CustomerUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>().HasMany(u => u.OrdersAsContractor)
                .WithRequired()
                .HasForeignKey(u => u.ContractorUserId)
                .WillCascadeOnDelete(false);
        }
    }
}
