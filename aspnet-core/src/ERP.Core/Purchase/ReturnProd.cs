using ERP.Purchase;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Purchase
{
    [Table("ReturnProds")]
    public class ReturnProd : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Reason { get; set; }

        public virtual long? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order OrderFk { get; set; }

    }
}