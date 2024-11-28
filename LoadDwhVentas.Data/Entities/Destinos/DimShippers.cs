using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVentas.Data.Entities.Destinos
{
    [Table("DimShippers", Schema = "dbo")]
    public class DimShippers
    {
        [Key]
        public int ShipperID { get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
    }
}
