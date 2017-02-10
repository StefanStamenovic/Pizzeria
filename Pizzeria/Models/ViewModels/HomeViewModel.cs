using Pizzeria.Models.Mongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> categories { get; set; }
    }
}