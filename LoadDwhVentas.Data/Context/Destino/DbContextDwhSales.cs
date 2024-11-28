using LoadDwhVentas.Data.Entities.Destinos;
using LoadDwhVentas.Data.Entities.Fuentes;
using Microsoft.EntityFrameworkCore;

namespace LoadDwhVentas.Data.Context.Destino
{
    public class DbContextDwhSales:DbContext
    {
        public DbContextDwhSales(DbContextOptions<DbContextDwhSales> dbContext):base(dbContext)
        {
            
        }
        public DbSet<DimCustomers> DimCustomers { get; set; }
        public DbSet<DimEmployees> DimEmployees { get; set; }
        public DbSet<DimProductsCategory> DimProductsCategory { get; set; }
        public DbSet<DimShippers> DimShippers { get; set; }
        public DbSet<DimDate> DimDate { get; set; }
        public DbSet<FactSales> FactSales { get; set; }
        public DbSet<FactCustomerServed> FactCustomerServed { get; set; }

       
    }
}
