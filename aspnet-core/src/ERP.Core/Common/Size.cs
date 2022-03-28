using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("Sizes")]
    public class Size : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(SizeConsts.MaxDisplayNameLength, MinimumLength = SizeConsts.MinDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        public virtual int SizeNum { get; set; }

    }
}