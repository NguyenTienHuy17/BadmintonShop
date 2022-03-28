using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Purchase.Dtos
{
    public class GetReturnProdForEditOutput
    {
        public CreateOrEditReturnProdDto ReturnProd { get; set; }

        public string OrderOrderCode { get; set; }

    }
}