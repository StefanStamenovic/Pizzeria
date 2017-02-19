using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using Pizzeria.Models.Mongo.Models;

namespace Pizzeria.Models.Mongo
{
    public class MongoDBDataProvider
    {
        private String connectionString = "mongodb://localhost/?safe=true";
        private String dbName = "pizzeria";
        private MongoClient client;
        private IMongoDatabase database;

        public MongoDBDataProvider()
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase(dbName);
        }


        #region Category

        public void CategoryCreate(string name)
        {
            IMongoCollection<Category> collection=database.GetCollection<Category>("categories");
            Category category = new Category {
                id = new ObjectId(),
                name = name,
                dishes = new List<Dish>(),
                supplements = new List<Supplement>()
            };
            collection.InsertOne(category);
        }

        public Category CategoryGet(string name)
        {
            IMongoCollection<Category> collection = database.GetCollection<Category>("categories");
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq("name", name);
            Category category = collection.Find(filter).SingleOrDefault();
            return category;
        }

        public List<Category> CategoryGetAll()
        {
            IMongoCollection<Category> collection = database.GetCollection<Category>("categories");
            List<Category> categories = collection.Find<Category>(new BsonDocument()).ToList<Category>();
            return categories;
        }

        #region Dish
        public void CategoryDishCreate(string name, string description, int price, string category)
        {
            if (CategoryGet(category) == null)
                return;
            IMongoCollection<Category> collection = database.GetCollection<Category>("categories");
            Dish dish = new Dish
            {
                id = new ObjectId(),
                name = name,
                description = description,
                price = price
            };
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq("name", category);
            UpdateDefinition<Category> update = Builders<Category>.Update.Push("dishes", dish);
            collection.FindOneAndUpdate(filter, update);
        }

        public void CategoryDishRemove(string name, string category)
        {
            if (CategoryGet(category) == null)
                return;
            IMongoCollection<Category> collection = database.GetCollection<Category>("categories");
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq("name", category);
            UpdateDefinition<Category> update = Builders<Category>.Update.PullFilter("dishes", Builders<Dish>.Filter.Eq("name", name));
            collection.FindOneAndUpdate(filter, update);
        }

        #endregion

        #region Supplement
        public void CategorySupplementCreate(string name, int price, string category)
        {
            if (CategoryGet(category) == null)
                return;
            IMongoCollection<Category> collection = database.GetCollection<Category>("categories");
            Supplement supplement = new Supplement
            {
                id = new ObjectId(),
                name = name,
                price = price
            };
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq("name", category);
            UpdateDefinition<Category> update = Builders<Category>.Update.Push("supplements", supplement);
            collection.FindOneAndUpdate(filter, update);
        }

        public void CategorySupplementRemove(string name, string category)
        {
            if (CategoryGet(category) == null)
                return;
            IMongoCollection<Category> collection = database.GetCollection<Category>("categories");
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq("name", category);
            UpdateDefinition<Category> update = Builders<Category>.Update.PullFilter("supplements", Builders<Supplement>.Filter.Eq("name", name));
            collection.FindOneAndUpdate(filter, update);
        }

        #endregion

        #endregion

        #region Location

        public void LocationCreate(string name)
        {

        }

        #endregion

        #region Order

        public void OrderCreate(string name, string adress, string phone, string price, string date, Location location,List<OrderedDish> orderedDish)
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("orders");
            Order order = new Order
            {
                name = name,
                adress = adress,
                phone = phone,
                price = price,
                date = date,
                location = location,
                orderedDish = orderedDish
            };
            collection.InsertOne(order);
        }

        public List<Order> OrderGetAll()
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("oreders");
            List<Order> orders = collection.Find<Order>(new BsonDocument()).ToList<Order>();
            return orders;
        }

        public void OrderDelete(ObjectId id)
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("orders");
            collection.DeleteOne(x => x.id == id);
        }

        #endregion

        public void DeleteAllData()
        {
            database.DropCollection("categories");
            database.DropCollection("orders");
        }
    }
}