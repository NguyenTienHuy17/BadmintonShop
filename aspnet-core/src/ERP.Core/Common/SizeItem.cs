using ERP.Entity;
using ERP.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("SizeItems")]
    public class SizeItem : AuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? SizeId { get; set; }

        [ForeignKey("SizeId")]
        public Size SizeFk { get; set; }

    }
}