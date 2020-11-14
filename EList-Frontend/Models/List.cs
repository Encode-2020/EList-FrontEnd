using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EList_Frontend.Models
{
    public class List
    {
        public int ListId { get; set; }
        public int UserId { get; set; }
        public string ListName { get; set; }
        public string ListLabel { get; set; }
        public string ListColor { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
