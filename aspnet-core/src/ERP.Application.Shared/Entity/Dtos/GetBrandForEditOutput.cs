using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entity.Dtos
{
    public class GetBrandForEditOutput
    {
        public CreateOrEditBrandDto Brand { get; set; }

        public string ImageName { get; set; }

    }
}