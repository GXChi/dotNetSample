using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Web.Models
{
    public class RSARequest
    {
        public string privateKey { get; set; }

        public string publicKey { get; set; }

        public string data { get; set; }

        public string sign { get; set; }
    }
}