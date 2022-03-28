using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class CategoryDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long? ImageId { get; set; }

    }
}