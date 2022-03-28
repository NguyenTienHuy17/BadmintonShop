using System;
using Abp.Application.Services.Dto;

namespace ERP.Common.Dtos
{
    public class BlogDto : EntityDto<long>
    {
        public string title { get; set; }

        public string content { get; set; }

    }
}