using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FTD.M3.API.Models;
using FTD.M3.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FTD.M3.API.Controllers
{
    [Route("api/[controller]")]
    public class M3Controller : Controller
    {
        private readonly IM3Service m3Service;

        public M3Controller(IM3Service service)
        {
            this.m3Service = service;
        }

        [HttpPost]
        public IActionResult Post([FromBody]RequestDto request)
        {
            return Json(m3Service.ExecuteM3(request));
        }

    }
}
