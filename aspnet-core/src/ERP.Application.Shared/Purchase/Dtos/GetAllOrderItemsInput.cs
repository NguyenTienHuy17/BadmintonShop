using Abp.Application.Services.Dto;
using System;

namespace ERP.Purchase.Dtos
{
    public class GetAllOrderItemsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string QuantityFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string OrderOrderCodeFilter { get; set; }

    }
}