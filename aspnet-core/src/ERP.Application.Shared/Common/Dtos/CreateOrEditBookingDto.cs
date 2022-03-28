using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditBookingDto : EntityDto<long?>
    {

        public DateTime Time { get; set; }

        [Required]
        [StringLength(BookingConsts.MaxDescriptionLength, MinimumLength = BookingConsts.MinDescriptionLength)]
        public string Description { get; set; }

    }
}