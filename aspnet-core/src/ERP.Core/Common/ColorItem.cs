using ERP.Entity;
using ERP.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("ColorItems")]
    public class ColorItem : AuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? ColorId { get; set; }

        [ForeignKey("ColorId")]
        public Color ColorFk { get; set; }

    }
}