using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Service_Camp.Models
{
    public class Server
    {
        [Key]
        public string ServerId { get; set; }
        public string Detail { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public string State { get; set; }

    }
}