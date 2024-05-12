using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    // TODO : ADD Table 
    [Table("Program_Type_Info")]
    public class ProgramTypeInfo
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Program")]
        public int? type_id { get; set; }

        public virtual ProgramType Program { get; set; }

        [ForeignKey("Service")]
        public int? Service_Class_id { get; set; } 
        public virtual Service Service { get; set; }
        public string? info { get; set; }
    }
}
