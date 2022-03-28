using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllColorsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ColorNameFilter { get; set; }

        public int? MaxColorCodeFilter { get; set; }
        public int? MinColorCodeFilter { get; set; }

    }
}