using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftraySolutions.Model
{
    internal class Collection
    {
        public Info info { get; set; }
        public List<Item> item { get; set; }
        public string id { get; set; }
        public string uid { get; set; }
    }
}
