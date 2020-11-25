﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EList_Frontend.Models.ViewModel
{
    public class ListItemModel
    {
        public List List { get; set; }
        public Item Item { get; set; }
       
        public List<List> Lists { get; set; }
        public List<Item> Items { get; set; }
        public List<Item> CompletedItems { get; set; }


    }
}
