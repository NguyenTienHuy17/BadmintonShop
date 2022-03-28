using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("Images")]
    public class Image : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ImageConsts.MaxNameLength, MinimumLength = ImageConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Url { get; set; }

        [StringLength(ImageConsts.MaxDescriptionLength, MinimumLength = ImageConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

    }
}