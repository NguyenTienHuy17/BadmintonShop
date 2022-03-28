using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditBlogDto : EntityDto<long?>
    {

        [Required]
        [StringLength(BlogConsts.MaxtitleLength, MinimumLength = BlogConsts.MintitleLength)]
        public string title { get; set; }

        [Required]
        public string content { get; set; }

    }
}