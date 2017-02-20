﻿using Pizzeria.Models.Mongo;
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

        [HttpGet]
        public ActionResult DeleteOrder(string id)
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            provider.OrderDelete(new MongoDB.Bson.ObjectId(id));
            return Redirect("/Management");
        }

        public ActionResult CreateSupplement(string category, string name, int price)
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            provider.CategorySupplementCreate(name, price, category);
            return Redirect("/Management");
        }
        public ActionResult DeleteSupplement(string category, string name)
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            provider.CategorySupplementRemove(name, category);
            return Redirect("/Management");
        }

        public ActionResult CreateDish(string category, string name,string description, int price)
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            provider.CategoryDishCreate(name, description, price, category);
            return Redirect("/Management");
        }

        public ActionResult DeleteDish(string name, string category)
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            provider.CategoryDishRemove(name, category);
            return Redirect("/Management");
        }

        public ActionResult CreateCategory(string name)
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            provider.CategoryCreate(name);
            return Redirect("/Management");
        }

        public ActionResult DeleteCategory(string id)
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            provider.CategoryDelete(new MongoDB.Bson.ObjectId(id));
            return Redirect("/Management");
        }
    }
}