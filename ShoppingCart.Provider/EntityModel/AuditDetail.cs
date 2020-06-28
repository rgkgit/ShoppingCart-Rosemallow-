using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Provider.EntityModel
{
    [Table("AuditDetail")]
    public class AuditDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AuditDetailID { get; set; }
        public int? UserId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string RequestURL { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Parameter { get; set; }
        public string ReturnValue { get; set; }
    }
}
