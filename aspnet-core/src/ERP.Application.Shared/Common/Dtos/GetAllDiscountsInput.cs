using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllDiscountsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DiscountCodeFilter { get; set; }

        public int? MaxDiscountFilter { get; set; }
        public int? MinDiscountFilter { get; set; }

        public string DescriptionFilter { get; set; }

    }
}