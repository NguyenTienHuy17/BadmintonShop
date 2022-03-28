using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class ColorDto : EntityDto<long>
    {
        public string ColorName { get; set; }

        public int ColorCode { get; set; }

    }
}