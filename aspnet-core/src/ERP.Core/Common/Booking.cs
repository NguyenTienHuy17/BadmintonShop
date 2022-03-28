using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common
{
    [Table("Bookings")]
    public class Booking : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime Time { get; set; }

        [Required]
        [StringLength(BookingConsts.MaxDescriptionLength, MinimumLength = BookingConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

    }
}