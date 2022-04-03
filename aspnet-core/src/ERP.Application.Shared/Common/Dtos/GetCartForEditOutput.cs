using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class GetCartForEditOutput
    {
        public CreateOrEditCartDto Cart { get; set; }

        public string ProductName { get; set; }

    }
}