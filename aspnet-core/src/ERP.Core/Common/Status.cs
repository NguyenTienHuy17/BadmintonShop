using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("Statuses")]
    public class Status : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(StatusConsts.MaxNameLength, MinimumLength = StatusConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

    }
}