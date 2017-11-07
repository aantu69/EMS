using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<State> States { get; set; }
    }
}