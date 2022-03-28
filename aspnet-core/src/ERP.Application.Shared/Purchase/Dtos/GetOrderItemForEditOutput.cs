using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Purchase.Dtos
{
    public class GetOrderItemForEditOutput
    {
        public CreateOrEditOrderItemDto OrderItem { get; set; }

        public string ProductName { get; set; }

        public string OrderOrderCode { get; set; }

    }
}