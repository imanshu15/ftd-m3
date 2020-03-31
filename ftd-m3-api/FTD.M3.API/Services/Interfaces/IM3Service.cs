using FTD.M3.API.Models;
using M3Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FTD.M3.API.Services.Interfaces
{
    public interface IM3Service
    {
        public M3Response ExecuteM3(RequestDto request);
    }
}
