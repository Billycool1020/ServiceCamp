using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service_Camp.Models
{
    public class ServerRentalRecord
    {
        public int Id { get; set; }
        public string ServerId { get; set; }
        public string Renter { get; set; }
        public bool IsActive { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}