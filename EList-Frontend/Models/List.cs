using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EList_Frontend.Models
{
    public enum ListColors {
        BG_DANGER,
        BG_INFO
    }

    public class List
    {
        public int ListId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "List Title is required")]
        public string ListName { get; set; }
        public string ListColor { get; set; }
        public DateTime ReminderDateTime { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
