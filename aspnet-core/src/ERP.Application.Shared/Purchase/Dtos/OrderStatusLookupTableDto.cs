﻿using Abp.Application.Services.Dto;

namespace ERP.Purchase.Dtos
{
    public class OrderStatusLookupTableDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }
    }
}