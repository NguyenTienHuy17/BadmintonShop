﻿using Abp.Application.Services.Dto;

namespace ERP.Entity.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}