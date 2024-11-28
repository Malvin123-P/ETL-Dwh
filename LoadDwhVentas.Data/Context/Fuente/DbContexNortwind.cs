using LoadDwhVentas.Data.Entities.Destinos;
using LoadDwhVentas.Data.Entities.Fuentes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDwhVentas.Data.Context.Fuente
{
    public class DbContexNortwind:DbContext
    {
        public DbContexNortwind(DbContextOptions<DbContexNortwind> dbContext): base(dbContext)
        {
            
        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Shippers> Shippers { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<ViewsServerdCustomer> ViewsServerdCustomer { get; set; }
        public DbSet<ViewsOrder> ViewsOrder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewsServerdCustomer>(Entities =>
            {
               Entities.HasNoKey()
                        .ToView("ViewsServerdCustomer", "dbo");
            });

            modelBuilder.Entity<ViewsOrder>(Entities =>
            {
                Entities.HasNoKey()
                        .ToView("ViewsOrder", "dbo");
            });

        }


    }
}
