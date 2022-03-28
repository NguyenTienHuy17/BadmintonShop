using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class CreateOrEditColorItemDto : EntityDto<long?>
    {

        public long? ProductId { get; set; }

        public long? ColorId { get; set; }

    }
}