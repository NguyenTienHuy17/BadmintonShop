using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class ImageDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

    }
}