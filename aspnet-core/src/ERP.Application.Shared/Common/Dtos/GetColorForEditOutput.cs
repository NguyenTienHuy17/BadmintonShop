﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.Dtos
{
    public class GetColorForEditOutput
    {
        public CreateOrEditColorDto Color { get; set; }

    }
}