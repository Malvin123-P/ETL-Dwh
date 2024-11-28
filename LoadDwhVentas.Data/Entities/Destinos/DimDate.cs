using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVentas.Data.Entities.Destinos
{
    [Table("DimDate", Schema = "dbo")]
    public class DimDate
    {
        [Key]
        public int ID { get; set; }
        public DateTime? Fecha { get; set; }
        [Column("NombreFecha", TypeName = "nvarchar(4000)")]
        public string? NombreFecha { get; set; }
        public int? Dia { get; set; }
        public int? Mes { get; set; }
        public int? Año { get; set; }

        [Column("Nombredia", TypeName = "nvarchar(4000)")]
        public string? NombreDia { get; set; }

        [Column("NombreMeses", TypeName = "nvarchar(4000)")]
        public string? NombreMeses { get; set; }

        [Column("NombreAño", TypeName = "nvarchar(4000)")]
        public string? NombreAño { get; set; }
   
    }

}
