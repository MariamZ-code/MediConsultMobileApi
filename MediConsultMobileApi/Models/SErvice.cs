using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("App_Service_Class")]

    public class Service
    {
        [Key]
        public int Service_Class_id { get; set; }
        public int? Service_Class_Parent_Id { get; set; }
        public string Service_Class_Name_En { get; set; }
        public string Service_Class_Name_Ar { get; set; }
        public virtual Service ParentService { get; set; }
        public virtual List<Service> ChildServices { get; set; }

        // TODO : Add To db 
        public string? image { get; set; }

        // TODO : Add To db 

        public int? hidden { get; set; }

        public virtual List<Policy> Policies { get; set;} = new List<Policy>();

    }
}
