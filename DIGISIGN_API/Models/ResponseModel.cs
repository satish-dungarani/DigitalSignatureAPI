using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIGISIGN_API.Models
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }

    public class InfoModel
    {
        public string encodedstring { get; set; }
        public string cordinatex1 { get; set; }
        public string cordinatey1 { get; set; }
        public string cordinatex2 { get; set; }
        public string cordinatey2 { get; set; }
        public string licenseKey { get; set; }
    }
}