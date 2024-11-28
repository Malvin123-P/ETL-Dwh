using LoadDwhVentas.Data.Context.Destino;
using LoadDwhVentas.Data.Context.Fuente;
using LoadDwhVentas.Data.Contratos;
using LoadDwhVentas.Data.Entities.Destinos;
using LoadDwhVentas.Data.Entities.Fuentes;
using LoadDwhVentas.Data.Results;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;

namespace LoadDwhVentas.Data.Services
{
    public class DataServicesWorker : IDataServicesWorker
    {
        private readonly DbContextDwhSales dbContextDwhSales;
        private readonly DbContexNortwind dbContexNortwind;
      public DataServicesWorker(DbContextDwhSales dbContextDwhSales, DbContexNortwind dbContexNortwind)
        {
            this.dbContextDwhSales = dbContextDwhSales;
            this.dbContexNortwind = dbContexNortwind;
        }
        public async Task<OperactionResult> LoadDwh()
        {
            OperactionResult result = new OperactionResult();
            try
            {
                // TODO: LLAMAR METODOS PRIVADOS
                await LoadDimEmployees();
                await LoadDimCustomers();
                await LoadDimProductsCategory();
                await LoadDimShippers();
                await LoadDimDate();
                await LoadFactCustomerServed();
                await LoadFactSales();
            }
            catch(Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el DWH. {ex.Message}";
            }
            return result;
        }

        private async Task<OperactionResult> LoadFactCustomerServed()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                var customServed = await dbContexNortwind.ViewsServerdCustomer.AsNoTracking().ToListAsync();

                // Limpiar tablas de FactCustomerServed antes de insertar nuevos datos
                int[] cust = customServed.Select(c => c.EmployeeID).ToArray();

                if (cust.Any())
                {
                         await dbContextDwhSales.FactCustomerServed
                        .Where(o => cust.Contains(o.EmployeeID))
                         .AsNoTracking().ExecuteDeleteAsync();
                }

                List<FactCustomerServed> factCustomerServeds = new List<FactCustomerServed>();

                // Cargar FactCustomerServed
                foreach (var item in customServed)
                {
                    var employee = await dbContextDwhSales.DimEmployees
                        .SingleOrDefaultAsync(e => e.EmployeeID == item.EmployeeID);

                    if (employee != null)
                    {
                        FactCustomerServed factCustomerAtendidos = new FactCustomerServed()
                        {
                            EmployeeID = employee.EmployeeID,
                            FullName = item.FullName.ToString(),
                            TotalCustomers = item.TotalCustomers
                        };

                        factCustomerServeds.Add(factCustomerAtendidos);
                    }
                }

                // Insertar los datos en la tabla FactCustomerServed
                await dbContextDwhSales.FactCustomerServed.AddRangeAsync(factCustomerServeds);
                await dbContextDwhSales.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el FactCustomerServed. {ex.Message}";
            }

            return result;
        }


        private async Task<OperactionResult> LoadFactSales()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                var order = await dbContexNortwind.ViewsOrder.AsNoTracking().ToListAsync();

                //// Limpiar tablas de FactOrder antes de insertar nuevos datos
                int[] orde = order.Select(o => o.OrderID).ToArray();

                 if (orde.Any())
                 {
                        await dbContextDwhSales.FactSales
                            .Where(o => orde.Contains(o.OrderID))
                           .AsNoTracking().ExecuteDeleteAsync();
                }

                List<FactSales> factSal = new List<FactSales>();


                //// Cargar FactSales
                foreach (var item in order)
                {
                    var customer = await dbContextDwhSales.DimCustomers
                        .SingleOrDefaultAsync(e => e.CustomerID == item.CustomerID);
                    var employee = await dbContextDwhSales.DimEmployees
                        .SingleOrDefaultAsync(e => e.EmployeeID == item.EmployeeID);
                    var product = await dbContextDwhSales.DimProductsCategory
                        .SingleOrDefaultAsync(e => e.ProductID == item.ProductID);
                    var shipper = await dbContextDwhSales.DimShippers
                        .SingleOrDefaultAsync(e => e.ShipperID == item.ShipperID);
                     // Ajuste del uso de fecha
                     var date = await dbContextDwhSales.DimDate .FirstOrDefaultAsync(e => e.Fecha == item.OrderDate);


                    if (employee != null && customer != null && product != null && shipper != null && date!=null)
                    {
                        FactSales factSales = new FactSales()
                        {
                            OrderID = item.OrderID,
                            CustomerID = customer.CustomerID,
                            CompanyName = customer.CompanyName,
                            EmployeeID = employee.EmployeeID,
                            FullName = employee.FullName,
                            ShipperID = shipper.ShipperID,
                            ShipperCompanyName = shipper.CompanyName,
                            ProductID = product.ProductID,
                            ProductName = product.ProductName,
                            ProductCount = item.ProductCount,
                            ShipCity = item.ShipCity,
                            OrderDate = date.Fecha,
                            Year = item.Year,
                            Month = item.Month,
                            TotalOrders = item.TotalOrders,
                            TotalSold = item.TotalSold
                        };

                        factSal.Add(factSales);
                    }
                }
                // Insertar los datos en el FactSales
                await dbContextDwhSales.FactSales.AddRangeAsync(factSal);
                await dbContextDwhSales.SaveChangesAsync();
                result.Success = true;
           

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el FactSales. {ex.Message}";
            }

            return result;
        }


        private async Task<OperactionResult> LoadDimDate()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                  var dimDates = await dbContexNortwind.Orders
                 .Where(order => order.OrderDate != null)
                 .OrderBy(order => order.OrderDate)
                 .Select(order => new DimDate
                 {
                     Fecha = order.OrderDate,
                     NombreFecha = order.OrderDate.ToString("dd/MM/yyyy"),
                     Dia = order.OrderDate.Day,
                     Mes = order.OrderDate.Month,
                     Año = order.OrderDate.Year,
                     NombreDia = order.OrderDate.ToString("dddd"),
                     NombreMeses = order.OrderDate.ToString("MMMM"),
                     NombreAño = order.OrderDate.ToString("yyyy"),
                    
                 })
                 .ToListAsync();

                // Limpiar tablas de DimDate antes de insertar nuevos datos
                int[] date =  dimDates.Select(dat => dat.ID).ToArray();

                if (date.Any())
                {
                    await dbContextDwhSales.DimDate.Where(d => date.Contains(d.ID)).AsNoTracking().ExecuteDeleteAsync();
                }

                // Insertar los datos en la tabla DimDate
                await dbContextDwhSales.DimDate.AddRangeAsync(dimDates);
                await dbContextDwhSales.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la DimDate. {ex.Message}";
            }
            return result;
        }

        private async Task<OperactionResult> LoadDimShippers()
        {
            OperactionResult operaction = new OperactionResult();
            try
            {
              
                var Shipper = await dbContexNortwind.Shippers.AsNoTracking().Select(shipper => new DimShippers()
                {
                    //Obtener los Shipper de la base de datos de norwind
                    ShipperID = shipper.ShipperID,
                    CompanyName = shipper.CompanyName,
                    Phone = shipper.Phone

                }).ToListAsync();

                // Limpiar tablas de DimShippers antes de insertar nuevos datos
                int[] shipper = Shipper.Select(ship => ship.ShipperID).ToArray();

                if (shipper.Any())
                {
                    await dbContextDwhSales.DimShippers.Where(s => shipper.Contains(s.ShipperID)).AsNoTracking().ExecuteDeleteAsync();
                }

                //Cargar Dimension
                await dbContextDwhSales.DimShippers.AddRangeAsync(Shipper);
                await dbContextDwhSales.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operaction.Success = false;
                operaction.Message = $"Error cargando la DimShippers. {ex.Message}";
            }

            return operaction;

        }

        private async  Task<OperactionResult> LoadDimProductsCategory()
        {
            OperactionResult operaction = new OperactionResult();

            try
            {
  

                //Obtner los Product de la base de datos de norwind
                var product = (from Products in dbContexNortwind.Products
                               join categories in dbContexNortwind.Categories
                               on Products.CategoryID equals categories.CategoryID
                               select new DimProductsCategory()
                               {
                                   ProductID = Products.ProductID,
                                   ProductName = Products.ProductName,
                                   CategoryID= Products.CategoryID,
                                   QuantityPerUnit = Products.QuantityPerUnit,
                                   UnitPrice = Products.UnitPrice,
                                   UnitsInStock = Products.UnitsInStock,
                                   UnitsOnOrder = Products.UnitsOnOrder,
                                   ReorderLevel = Products.ReorderLevel,
                                   Discontinued = Products.Discontinued,
                                   CategoryName = categories.CategoryName,
                                   DescriptionCategory = categories.Description

                               }).AsNoTracking().ToList();

                // Limpiar tablas de DimProductsCategory antes de insertar nuevos datos
                int[] produc = product.Select(p => p.ProductID).ToArray();

                if (produc.Any())
                {
                    await dbContextDwhSales.DimProductsCategory.Where(p => produc.Contains(p.ProductID)).AsNoTracking().ExecuteDeleteAsync();
                }

                //Cargar Dimension
                await dbContextDwhSales.DimProductsCategory.AddRangeAsync(product);
                await dbContextDwhSales.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                operaction.Success = false;
                operaction.Message = $"Error cargando la DimProductsCategory. {ex.Message}";
            }

            return operaction;
        }

        private async Task<OperactionResult> LoadDimCustomers()
        {
            OperactionResult operaction = new OperactionResult();
            try
            {
                // Limpiar tablas de DimCustomers antes de insertar nuevos datos
                await dbContextDwhSales.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [DimCustomers]");

                var customer = await dbContexNortwind.Customers.AsNoTracking().Select(customer => new DimCustomers()
                {
                    //Obtner los Customer de la base de datos de norwind
                    CustomerID = customer.CustomerID,
                    CompanyName = customer.CompanyName,
                    ContactName = customer.ContactName,
                    ContactTitle = customer.ContactTitle,
                    Address = customer.Address,
                    City = customer.City,
                    Region = customer.Region,
                    PostalCode = customer.PostalCode,
                    Country = customer.Country,
                    Phone = customer.Phone,
                    Fax = customer.Fax

                }).ToListAsync();

                // Limpiar tablas de DimCustomers antes de insertar nuevos datos
                string[] cust = customer.Select(c => c.CustomerID).ToArray();

                if (cust.Any())
                {
                    await dbContextDwhSales.DimCustomers.Where(c => cust.Contains(c.CustomerID)).AsNoTracking().ExecuteDeleteAsync();
                }

                //Cargar Dimension
                await dbContextDwhSales.DimCustomers.AddRangeAsync(customer);
                await dbContextDwhSales.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operaction.Success = false;
                operaction.Message = $"Error cargando la DimCustomers. {ex.Message}";
            }
            return operaction;
        }


        private async Task<OperactionResult> LoadDimEmployees()
        {
            OperactionResult operaction = new OperactionResult();
            try
            {
              

                var employees = await dbContexNortwind.Employees.AsNoTracking().Select(employee => new DimEmployees()
                {
                    // Obtener los Employees de la base de datos de Northwind
                    EmployeeID = employee.EmployeeID,
                    FullName = string.Concat(employee.FirstName, " ", employee.LastName),
                    Title = employee.Title,
                    City = employee.City,
                    Region = employee.Region,
                    Country = employee.Country,
                    HomePhone = employee.HomePhone,
                    Extension = employee.Extension

                }).ToListAsync();

                // Limpiar tablas de DimEmployees antes de insertar nuevos datos
                int[] emple = employees.Select(e => e.EmployeeID).ToArray();

                if (emple.Any())
                {
                    await dbContextDwhSales.DimEmployees.Where(e => emple.Contains(e.EmployeeID)).AsNoTracking().ExecuteDeleteAsync();
                }


                //Cargar Dimension
                await dbContextDwhSales.DimEmployees.AddRangeAsync(employees);
                await dbContextDwhSales.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operaction.Success = false;
                operaction.Message = $"Error cargando la DimEmployees. {ex.Message}";
            }

            return operaction;



        }
    }
    
}
