using Pizzeria.Models;
using Pizzeria.Models.Mongo;
using Pizzeria.Models.ViewModels;
using Pizzeria.Models.Mongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;


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
            model.districts = provider.DistrictGetAll();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProcessOrder(OrderFormViewModel form)
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();
            List<OrderedDish> cart = (List<OrderedDish>)Session["Cart"];
            List<Restaurant> restaurants = provider.DistrictGet(form.district).restaurants;
            Restaurant restaurant = restaurants.ElementAt(new Random().Next() % restaurants.Count);
            provider.OrderCreate(form.name, form.adress, form.phone, form.price, DateTime.Now.ToString("dd-MM-yyyy"), restaurant.id, cart);
            cart.RemoveRange(0, cart.Count);
            return new HttpStatusCodeResult(200);
        }

        public JsonResult AddToCart(CartEntryViewModel entry)
        {
            MongoDBDataProvider provider = new MongoDBDataProvider();

            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<OrderedDish>();
            }

            OrderedDish orderedDish = new OrderedDish();
            Category tmpCat = provider.CategoryGet(entry.category);
            orderedDish.price = 0;
            orderedDish.dish = tmpCat.dishes.Where(x => x.name == entry.dish).SingleOrDefault();
            orderedDish.quantity = entry.quantity;
            if (entry.supplements == null)
                orderedDish.supplements = null;
            else
            {
                orderedDish.supplements = new List<Supplement>();
                foreach (string supName in entry.supplements)
                {
                    foreach (Supplement catSup in tmpCat.supplements)
                    {
                        if (supName == catSup.name)
                        {
                            Supplement tmpSup = new Supplement();
                            tmpSup.name = catSup.name;
                            tmpSup.price = catSup.price;
                            orderedDish.supplements.Add(tmpSup);
                            orderedDish.price += tmpSup.price;
                        }
                    }
                }
            }
            
            orderedDish.price += orderedDish.dish.price * orderedDish.quantity;
            List<OrderedDish> cart = (List<OrderedDish>)Session["Cart"];
            cart.Add(orderedDish);
            return Json(cart);
        }

        public JsonResult RemoveFromCart(int no)
        {
            if (Session["Cart"] == null)
            {
                return null;
            }
            List<OrderedDish> cart = (List<OrderedDish>)Session["Cart"];
            if(no >= 0 && no < cart.Count)
            {
                cart.RemoveAt(no);
            }
            return Json(cart);
        }

        public JsonResult GetCart()
        {
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<OrderedDish>();
            }
            List<OrderedDish> cart = (List<OrderedDish>)Session["Cart"];
            return Json(cart);
        }
    }
}