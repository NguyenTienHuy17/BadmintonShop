using ERP.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Entity
{
    [Table("Brands")]
    public class Brand : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(BrandConsts.MaxNameLength, MinimumLength = BrandConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(BrandConsts.MaxCountryLength, MinimumLength = BrandConsts.MinCountryLength)]
        public virtual string Country { get; set; }

        public virtual string Description { get; set; }
        public virtual string ImageUrl { get; set; }


    }
}