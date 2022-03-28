using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class DiscountDto : EntityDto<long>
    {
        public string DiscountCode { get; set; }

        public int DiscountNum { get; set; }

        public string Description { get; set; }

    }
}