using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Service_Camp.Models
{
    public class UserServer
    {
        [Key]
        public string UserId { get; set; }
        public virtual ICollection<Server> Servers { get; set; }
    }
}