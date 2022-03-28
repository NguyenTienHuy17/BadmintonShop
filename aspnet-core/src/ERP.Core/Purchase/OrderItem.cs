using ERP.Entity;
using ERP.Purchase;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Purchase
{
    [Table("OrderItems")]
    public class OrderItem : AuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Quantity { get; set; }

        public virtual long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order OrderFk { get; set; }

    }
}