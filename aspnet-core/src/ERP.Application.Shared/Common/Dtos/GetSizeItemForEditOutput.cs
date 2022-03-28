using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class GetSizeItemForEditOutput
    {
        public CreateOrEditSizeItemDto SizeItem { get; set; }

        public string ProductName { get; set; }

        public string SizeDisplayName { get; set; }

    }
}