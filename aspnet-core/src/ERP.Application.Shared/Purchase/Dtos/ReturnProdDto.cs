using System;
using Abp.Application.Services.Dto;

namespace ERP.Purchase.Dtos
{
    public class ReturnProdDto : EntityDto<long>
    {
        public string Reason { get; set; }

        public long? OrderId { get; set; }

    }
}