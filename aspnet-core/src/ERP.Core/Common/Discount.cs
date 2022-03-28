using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("Discounts")]
    public class Discount : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(DiscountConsts.MaxDiscountCodeLength, MinimumLength = DiscountConsts.MinDiscountCodeLength)]
        public virtual string DiscountCode { get; set; }

        [Range(DiscountConsts.MinDiscountValue, DiscountConsts.MaxDiscountValue)]
        public virtual int DiscountNum { get; set; }

        [StringLength(DiscountConsts.MaxDescriptionLength, MinimumLength = DiscountConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

    }
}