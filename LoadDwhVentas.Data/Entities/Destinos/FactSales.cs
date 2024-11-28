using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVentas.Data.Entities.Destinos
{
    [Table("FactSales", Schema = "dbo")]
    public class FactSales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KeyId { get; set; }
        public int OrderID { get; set; }
        public string? CustomerID { get; set; }
        public string? CompanyName { get; set; }
        public int EmployeeID { get; set; }
        public string? FullName { get; set; }
        public int ShipperID { get; set; }
        public string? ShipperCompanyName { get; set; }
        public int? ProductID { get; set; }
        public string? ProductName { get; set; }
        public int ProductCount { get; set; }
        public string? ShipCity { get; set; }
        public DateTime? OrderDate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSold { get; set; }

    }
}
