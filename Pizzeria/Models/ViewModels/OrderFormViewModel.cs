using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models.ViewModels
{
    public class OrderFormViewModel
    {
        public string name { get; set; }
        public string adress { get; set; }
        public string phone { get; set; }
        public string price { get; set; }
        public string district { get; set; }
    }
}