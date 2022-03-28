using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class GetCategoryForEditOutput
    {
        public CreateOrEditCategoryDto Category { get; set; }

        public string ImageName { get; set; }

    }
}