using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllColorItemsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ColorColorNameFilter { get; set; }

    }
}