using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarGo.Models.Info
{
    public class Customer
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public Dictionary<string,Ownership> Ownerships { get; set; }
    }
}