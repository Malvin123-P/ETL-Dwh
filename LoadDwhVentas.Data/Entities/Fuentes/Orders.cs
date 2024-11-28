using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVentas.Data.Entities.Fuentes
{
    [Table("Orders", Schema = "dbo")]
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
