using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EList_Frontend.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public DateTime Datetime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }


    }
}
