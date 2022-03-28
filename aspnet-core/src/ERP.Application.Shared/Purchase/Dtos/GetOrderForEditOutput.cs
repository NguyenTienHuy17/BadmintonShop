using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Purchase.Dtos
{
    public class GetOrderForEditOutput
    {
        public CreateOrEditOrderDto Order { get; set; }

        public string StatusName { get; set; }

        public string DiscountDiscountCode { get; set; }

    }
}