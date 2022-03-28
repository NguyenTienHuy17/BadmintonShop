using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("Blogs")]
    public class Blog : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(BlogConsts.MaxtitleLength, MinimumLength = BlogConsts.MintitleLength)]
        public virtual string title { get; set; }

        [Required]
        public virtual string content { get; set; }

    }
}