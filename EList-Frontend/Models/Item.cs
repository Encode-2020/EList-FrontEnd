using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EList_Frontend.Models
{
    public class Item
    {
        public string ItemId { get; set; }
        public string ListId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime FinalDate { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
