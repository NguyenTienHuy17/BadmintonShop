using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ERP.Entity.Dtos;

namespace ERP.Purchase.Dtos
{
    public class GetOrderItemForEditOutput
    {
        public CreateOrEditOrderItemDto OrderItem { get; set; }

        public string ProductName { get; set; }

        public string OrderOrderCode { get; set; }

    }
}