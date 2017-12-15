using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Service_Camp.Models
{
    public class ApplyRecord
    {
        public int Id { get; set; }

        [ForeignKey("Server")]
        public string ServerId { get; set; }

        public string Applicant { get; set; }
        public string Admin { get; set; }

        public DateTime CreateDate { get; set; }

        public string States { get; set; }

        public DateTime? HandleDate { get; set; }
        public virtual Server Server { get; set; }

    }
}