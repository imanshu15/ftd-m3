using System;
using System.Collections.Generic;
using System.Text;

namespace M3Service.Model
{
     public class ClientConfiguration
    {

        public string ContentType { get; set; }

        public string Accept { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Cookie { get; set; }

        public string ServiceUrl { get; set; }
    }
}
