using Abp.Application.Services.Dto;
using System;

namespace ERP.Common.Dtos
{
    public class GetAllProductImagesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ImageNameFilter { get; set; }

    }
}