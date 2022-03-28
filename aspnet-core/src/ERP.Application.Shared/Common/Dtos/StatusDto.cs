using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class StatusDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

    }
}