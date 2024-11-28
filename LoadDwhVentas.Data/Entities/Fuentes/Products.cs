using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVentas.Data.Entities.Fuentes
{
    [Table("Products", Schema = "dbo")]
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int? CategoryID { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public bool? Discontinued { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }

    }
}
