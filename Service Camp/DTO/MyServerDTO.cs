using Service_Camp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service_Camp.DTO
{
    public class MyServerDTO
    {
        public List<Server> MyServer { get; set; }
        public List<ApplyRecord> ApplyRecord { get; set; }
        public List<Server> AvaliableServer { get; set; }
    }
}