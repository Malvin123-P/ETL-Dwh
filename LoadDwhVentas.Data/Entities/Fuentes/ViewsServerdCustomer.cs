using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadDwhVentas.Data.Entities.Fuentes
{
    public class ViewsServerdCustomer
    {
        public int EmployeeID { get; set; }
        public string? FullName { get; set; }
        public int TotalCustomers { get; set; }
    }
}
