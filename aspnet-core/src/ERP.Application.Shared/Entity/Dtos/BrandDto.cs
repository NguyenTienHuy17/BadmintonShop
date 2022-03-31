using System;
using Abp.Application.Services.Dto;

namespace ERP.Entity.Dtos
{
    public class BrandDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }


    }
}