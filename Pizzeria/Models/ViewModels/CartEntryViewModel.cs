using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models.ViewModels
{
    public class CartEntryViewModel
    {
        public string category { get; set; }
        public string dish { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public List<string> supplements { get; set; }
    }
}