﻿using Abp.Application.Services.Dto;

namespace ERP.Purchase.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}