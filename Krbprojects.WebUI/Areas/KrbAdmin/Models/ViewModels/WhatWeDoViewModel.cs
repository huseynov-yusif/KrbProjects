using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Krbprojects.WebUI.Areas.KrbAdmin.Models.ViewModels
{
    public class WhatWeDoViewModel
    {

        public int? Id { get; set; }
        public string Name_AZ { get; set; }
        public string Name_EN { get; set; }
        public string Name_RU { get; set; }
        public string Description_AZ { get; set; }
        public string Description_EN { get; set; }
        public string Description_RU { get; set; }
        public string ImagePath { get; set; }
        public IFormFile file { get; set; }
        public string fileTemp { get; set; }
    }
}
