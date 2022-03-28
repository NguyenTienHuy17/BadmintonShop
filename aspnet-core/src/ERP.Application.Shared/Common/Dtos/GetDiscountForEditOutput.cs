using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class GetDiscountForEditOutput
    {
        public CreateOrEditDiscountDto Discount { get; set; }

    }
}