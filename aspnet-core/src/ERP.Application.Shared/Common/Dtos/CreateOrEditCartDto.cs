using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditCartDto : EntityDto<long?>
    {

        public int quantity { get; set; }

        public long ProductId { get; set; }

    }
}