using System;
using System.Collections.Generic;
using System.Text;

namespace M3Service.Model
{
    public class M3Response
    {
        public M3Response()
        {
            this.Success = true;
        }
        public bool Success { get; set; }

        public string Message { get; set; }

        public dynamic Data { get; set; }
    }
}
