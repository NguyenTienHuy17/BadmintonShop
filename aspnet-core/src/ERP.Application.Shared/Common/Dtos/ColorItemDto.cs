using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class ColorItemDto : EntityDto<long>
    {

        public long? ProductId { get; set; }

        public long? ColorId { get; set; }

    }
}