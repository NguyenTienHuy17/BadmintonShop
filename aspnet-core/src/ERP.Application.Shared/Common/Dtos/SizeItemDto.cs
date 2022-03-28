using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class SizeItemDto : EntityDto<long>
    {

        public long? ProductId { get; set; }

        public long? SizeId { get; set; }

    }
}