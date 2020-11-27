using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EList_Frontend.Models
{
    public enum ListColors {
        BG_DANGER,
        BG_INFO,
        BG_PRIMARY,
        BG_SUCCESS,
        BG_SECONDARY,
        BG_WARNING
    }

    public class List
    {
        public int ListId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "List Title is required")]
        public string ListName { get; set; }
        public ListColors ListColor { get; set; }
        public string color { get; set; }
        public DateTime LastEdited { get; set; }
        public DateTime ReminderDateTime { get; set; }
        public List<Item> Items { get; set; }
    }
}
