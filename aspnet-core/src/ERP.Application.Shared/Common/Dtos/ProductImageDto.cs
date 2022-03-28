using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class ProductImageDto : EntityDto<long>
    {

        public long? ProductId { get; set; }

        public long? ImageId { get; set; }

    }
}