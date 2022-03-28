using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("Colors")]
    public class Color : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ColorConsts.MaxColorNameLength, MinimumLength = ColorConsts.MinColorNameLength)]
        public virtual string ColorName { get; set; }

        public virtual int ColorCode { get; set; }

    }
}