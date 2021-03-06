﻿using MongoDB.Bson;
using Pizzeria.Models.Mongo;
using Pizzeria.Models.Mongo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Pizzeria.Models
{
    public class DBInitializer
    {
        string categoryFilePath = @"~/category.txt";
        string orderFilePath = @"~/orders.txt";
        string districtFilePath = @"~/districts.txt";
        public DBInitializer()
        {
        }

        public void Initialize()
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy");

            MongoDBDataProvider dbMongo = new MongoDBDataProvider();

            StreamReader stream;
            //Check is database initialized
            if (dbMongo.CategoryGetAll().Count != 0)
                return;

            DeleteAllData();

            try
            {
                stream = new StreamReader(HostingEnvironment.MapPath(categoryFilePath));
                while (!stream.EndOfStream)
                {
                    Category category = new Category();
                    category.name= stream.ReadLine();
                    stream.ReadLine();
                    int numofdishes = Convert.ToInt32(stream.ReadLine());
                    List<Dish> dishes = new List<Dish>();
                    for(int i=0;i<numofdishes;i++)
                    {
                        Dish dish = new Dish();
                        dish.name = stream.ReadLine();
                        dish.description = stream.ReadLine();
                        dish.price = Convert.ToInt32(stream.ReadLine());
                        dishes.Add(dish);
                    }
                    int numofsupplements = Convert.ToInt32(stream.ReadLine());
                    List<Supplement> supplements = new List<Supplement>();
                    for (int i = 0; i < numofsupplements; i++)
                    {
                        Supplement supplement = new Supplement();
                        supplement.name = stream.ReadLine();
                        supplement.price = Convert.ToInt32(stream.ReadLine());
                        supplements.Add(supplement);
                    }
                    category.dishes = dishes;
                    category.supplements = supplements;
                    dbMongo.CategoryCreate(category.name);
                    foreach(Dish dish in category.dishes)
                        dbMongo.CategoryDishCreate(dish.name, dish.description, dish.price, category.name);
                    foreach (Supplement supplement in category.supplements)
                        dbMongo.CategorySupplementCreate(supplement.name, supplement.price, category.name);
                }
                stream.Close();
            }
            catch (Exception e)
            {

            }

           

            try
            {
                stream = new StreamReader(HostingEnvironment.MapPath(districtFilePath));
                while (!stream.EndOfStream)
                {
                    District district = new District();
                    district.name = stream.ReadLine();
                    int numofRestaurants = Convert.ToInt32(stream.ReadLine());
                    district.restaurants = new List<Restaurant>();
                    for (int i = 0; i < numofRestaurants; i++)
                    {
                        Restaurant restaurant = new Restaurant();
                        restaurant.name = stream.ReadLine();
                        restaurant.address = stream.ReadLine();
                        restaurant.coordinates = new double[2];
                        restaurant.coordinates[0] = Convert.ToDouble(stream.ReadLine());
                        restaurant.coordinates[1] = Convert.ToDouble(stream.ReadLine());
                        district.restaurants.Add(restaurant);
                    } 
                    dbMongo.DistrictCreate(district);
                }
                stream.Close();
            }
            catch (Exception e)
            {

            }
            dbMongo.DelivererCreate("Piter Parker");
            dbMongo.DelivererCreate("Tony Stark");
            dbMongo.DelivererCreate("Bruce Wayne");

            try
            {
                stream = new StreamReader(HostingEnvironment.MapPath(orderFilePath));
                while (!stream.EndOfStream)
                {
                    Order order = new Order();
                    order.name = stream.ReadLine();
                    order.adress = stream.ReadLine();
                    order.phone = stream.ReadLine();
                    order.price = stream.ReadLine();
                    OrderedDish orderDish = new OrderedDish();
                    Dish dish = new Dish();
                    dish.name = stream.ReadLine();
                    dish.description = stream.ReadLine();
                    dish.price = Convert.ToInt32(stream.ReadLine());
                    orderDish.dish = dish;
                    orderDish.quantity = Convert.ToInt32(stream.ReadLine());
                    int numofsupplements = Convert.ToInt32(stream.ReadLine());
                    List<Supplement> supplements = new List<Supplement>();
                    for (int i = 0; i < numofsupplements; i++)
                    {
                        Supplement supplement = new Supplement();
                        supplement.name = stream.ReadLine();
                        supplement.price = Convert.ToInt32(stream.ReadLine());
                        supplements.Add(supplement);
                    }
                    orderDish.supplements = supplements;
                    order.orderedDish = new List<OrderedDish> { orderDish };
                    List<Restaurant> restaurants = dbMongo.DistrictGet("Palilula").restaurants;
                    Restaurant restaurant = restaurants.ElementAt(new Random().Next() % restaurants.Count);
                    dbMongo.OrderCreate(order.name, order.adress, order.phone, order.price, date, restaurant.id, order.orderedDish);
                }
                stream.Close();
            }
            catch (Exception e)
            {

            }
        }

        public void DeleteAllData()
        {
            MongoDBDataProvider dbMongo = new MongoDBDataProvider();
            dbMongo.DeleteAllData();
        }
    }
}