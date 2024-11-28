using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVentas.Data.Entities.Fuentes
{
    [Table("Shippers", Schema = "dbo")]
    public class Shippers
    {
        [Key]
        public int ShipperID { get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
    
    }
}
