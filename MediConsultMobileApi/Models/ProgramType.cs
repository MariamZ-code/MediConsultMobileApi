using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_Program_Type")]
    public class ProgramType
    {
        [Key]
        public int type_id { get; set; }
        public string Type_Name_En { get; set; }
        public string Type_Name_Ar { get; set; }
        public int is_deleted { get; set; }
        public virtual List<ProgramTypeInfo> programTypeInfos { get; set; } = new List<ProgramTypeInfo>();
    }
}
