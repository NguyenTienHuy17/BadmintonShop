using ERP.Common;
using ERP.Entity;
using ERP.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Entity
{
    [Table("Products")]
    public class Product : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ProductConsts.MaxNameLength, MinimumLength = ProductConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(ProductConsts.MaxMadeInLength, MinimumLength = ProductConsts.MinMadeInLength)]
        public virtual string MadeIn { get; set; }

        [StringLength(ProductConsts.MaxCodeLength, MinimumLength = ProductConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        public virtual long Price { get; set; }

        public virtual int InStock { get; set; }

        public virtual string Description { get; set; }

        [StringLength(ProductConsts.MaxTitleLength, MinimumLength = ProductConsts.MinTitleLength)]
        public virtual string Title { get; set; }

        public virtual long? ImageId { get; set; }

        [ForeignKey("ImageId")]
        public Image ImageFk { get; set; }

        public virtual long? BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand BrandFk { get; set; }

        public virtual long? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category CategoryFk { get; set; }

    }
}