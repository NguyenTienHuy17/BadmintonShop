using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllBlogsForExcelInput
    {
        public string Filter { get; set; }

        public string titleFilter { get; set; }

        public string contentFilter { get; set; }

    }
}