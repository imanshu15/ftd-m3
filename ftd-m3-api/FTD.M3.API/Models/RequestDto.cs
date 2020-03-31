using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FTD.M3.API.Models
{
    public class RequestDto
    {
        public string Program { get; set; }

        public string Transaction { get; set; }

        public dynamic Param { get; set; }

        public string[] Output { get; set; }

        public bool OutputAll { get; set; }

        public dynamic Filter { get; set; }

        public string[] Sort { get; set; }

        public bool OrderByDesc { get; set; }

        //public bool Pageable { get; set; }

        //public long Take { get; set; }

        //public long Skip { get; set; }
    }
}
