using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entity.Dtos
{
    public class GetProductForEditOutput
    {
        public CreateOrEditProductDto Product { get; set; }

        public string ImageName { get; set; }

        public string BrandName { get; set; }

        public string CategoryName { get; set; }

    }
}