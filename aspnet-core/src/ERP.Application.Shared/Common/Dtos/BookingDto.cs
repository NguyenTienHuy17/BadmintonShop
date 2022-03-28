using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class BookingDto : EntityDto<long>
    {
        public DateTime Time { get; set; }

        public string Description { get; set; }

    }
}