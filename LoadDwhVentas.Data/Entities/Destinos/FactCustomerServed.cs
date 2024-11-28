using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVentas.Data.Entities.Destinos
{
    [Table("FactCustomerServed", Schema = "dbo")]
    public class FactCustomerServed
    {
        [Key]
        public int EmployeeID { get; set; }
        public string? FullName { get; set; }
        public int TotalCustomers { get; set; }
    }
}
