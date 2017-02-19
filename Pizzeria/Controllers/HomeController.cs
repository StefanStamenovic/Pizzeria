using Pizzeria.Models;
using Pizzeria.Models.Mongo;
using Pizzeria.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizzeria.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            HomeViewModel model = new HomeViewModel();

            DBInitializer initializer = new DBInitializer();
            initializer.DeleteAllData();
            initializer.Initialize();

            model.categories = provider.CategoryGetAll();
            return View(model);
        }
    }
}