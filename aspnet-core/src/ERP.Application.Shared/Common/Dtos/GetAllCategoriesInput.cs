﻿using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllCategoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string ImageNameFilter { get; set; }

    }
}