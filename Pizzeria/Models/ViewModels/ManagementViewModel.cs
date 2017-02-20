using Pizzeria.Models.Mongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models.ViewModels
{
    public class ManagementViewModel
    {
        public List<Order> orders { get; set; }
        public List<Order> history { get; set; }
        public List<Deliverer> deliverers { get; set; }

        public List<Category> categories { get; set; }
    }
}