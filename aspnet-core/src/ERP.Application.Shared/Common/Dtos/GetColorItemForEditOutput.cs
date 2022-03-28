using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class GetColorItemForEditOutput
    {
        public CreateOrEditColorItemDto ColorItem { get; set; }

        public string ProductName { get; set; }

        public string ColorColorName { get; set; }

    }
}