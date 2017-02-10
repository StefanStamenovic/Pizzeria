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

        #region Dish

        public void DishCreate(string name, string description, int price, string category)
        {
            if (CategoryGet(category) == null)
                return;
            IMongoCollection<Category> collection = database.GetCollection<Category>("categories");
            Dish dish = new Dish
            {
                name = name,
                description = description,
                price = price
            };
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq("name", category);
            UpdateDefinition<Category> update = Builders<Category>.Update.AddToSet("dishes", dish);
            collection.UpdateOne(filter, update);
        }

        #endregion

        #region Category

        public void CategoryCreate(string name, string imageurl)
        {
            IMongoCollection<Category> collection=database.GetCollection<Category>("categories");
            Category category = new Category{
                name = name,
                imageUrl = imageurl,
                dishes = null,
                supplements = null
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

        #endregion

        #region Supplement

        public void SupplementCreate(string name, int price, string category)
        {
            if (CategoryGet(category) == null)
                return;
            IMongoCollection<Category> collection = database.GetCollection<Category>("categories");
            Supplement supplement = new Supplement
            {
                name = name,
                price = price
            };
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq("name", category);
            UpdateDefinition<Category> update = Builders<Category>.Update.AddToSet("supplements", supplement);
            collection.UpdateOne(filter, update);
        }

        #endregion

        #region Location

        public void LocationCreate(string name)
        {

        }

        #endregion

        #region Order

        public void OrderCreate(string name, string adress, string phone, string price, string date, Location location)
        {

        }

        #endregion


        public void Initialize()
        {
            database.CreateCollection("dishes");
            database.CreateCollection("supplements");
        }


    }
}