using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EList_Frontend.Models
{
    public class Item
    {
        public string ItemId { get; set; }
        public int ListId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted {get; set;}
        public string Url { get; set; }

    }
}
