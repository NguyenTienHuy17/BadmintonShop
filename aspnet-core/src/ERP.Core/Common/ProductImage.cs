using ERP.Entity;
using ERP.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("ProductImages")]
    public class ProductImage : AuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        [Required]
        [StringLength(ImageConsts.MaxNameLength, MinimumLength = ImageConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Url { get; set; }

        [StringLength(ImageConsts.MaxDescriptionLength, MinimumLength = ImageConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

    }
}