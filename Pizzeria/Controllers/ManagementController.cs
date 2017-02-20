using Pizzeria.Models.Mongo;
using Pizzeria.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizzeria.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        public ActionResult Index()
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            ManagementViewModel model = new ManagementViewModel();

            model.orders = provider.OrderGetAll();
            model.history = provider.HistoryOrderGetAll();
            model.deliverers = provider.DelivererGetAll();
            model.categories = provider.CategoryGetAll();
            return View(model);
        }
    }
}