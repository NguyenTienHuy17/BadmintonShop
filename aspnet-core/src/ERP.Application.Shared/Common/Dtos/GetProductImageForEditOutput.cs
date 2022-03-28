using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class GetProductImageForEditOutput
    {
        public CreateOrEditProductImageDto ProductImage { get; set; }

        public string ProductName { get; set; }

        public string ImageName { get; set; }

    }
}