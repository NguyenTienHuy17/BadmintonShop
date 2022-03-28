using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class GetBlogForEditOutput
    {
        public CreateOrEditBlogDto Blog { get; set; }

    }
}