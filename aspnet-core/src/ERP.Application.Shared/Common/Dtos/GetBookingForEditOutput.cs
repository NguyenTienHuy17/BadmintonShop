using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class GetBookingForEditOutput
    {
        public CreateOrEditBookingDto Booking { get; set; }

    }
}