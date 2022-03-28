using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class SizeDto : EntityDto<long>
    {
        public string DisplayName { get; set; }

        public int SizeNum { get; set; }

    }
}