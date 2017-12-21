using Service_Camp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service_Camp.DTO
{
    public class AdminDTO
    {
        public List<Server> AllServers { get; set; }
        public List<ApplyRecord> ApplyRecords { get; set; }
        public List<Server> DirtyServers { get; set; }
        public List<Server> RentendServers { get; set; }

        public Server Server { get; set; }
    }
}