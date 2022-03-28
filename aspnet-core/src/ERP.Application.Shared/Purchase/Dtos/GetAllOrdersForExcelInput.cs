using Abp.Application.Services.Dto;
using System;

namespace ERP.Purchase.Dtos
{
    public class GetAllOrdersForExcelInput
    {
        public string Filter { get; set; }

        public string OrderCodeFilter { get; set; }

        public string TotalPriceFilter { get; set; }

        public string ShippingAddressFilter { get; set; }

        public string ShippingNumberFilter { get; set; }

        public long? MaxDiscountAmountFilter { get; set; }
        public long? MinDiscountAmountFilter { get; set; }

        public long? MaxActualPriceFilter { get; set; }
        public long? MinActualPriceFilter { get; set; }

        public string StatusNameFilter { get; set; }

        public string DiscountDiscountCodeFilter { get; set; }

    }
}