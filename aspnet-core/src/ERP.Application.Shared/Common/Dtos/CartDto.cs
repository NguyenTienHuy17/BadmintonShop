using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class CartDto : EntityDto<long>
    {
        public int quantity { get; set; }

        public long ProductId { get; set; }

    }
}