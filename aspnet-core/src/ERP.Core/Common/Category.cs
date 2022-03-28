using ERP.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("Categories")]
    public class Category : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(CategoryConsts.MaxNameLength, MinimumLength = CategoryConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual long? ImageId { get; set; }

        [ForeignKey("ImageId")]
        public Image ImageFk { get; set; }

    }
}