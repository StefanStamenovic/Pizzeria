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

        #region Order

        public void OrderCreate(string name, string adress, string phone, string price, string date,List<OrderedDish> orderedDish)
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("orders");
            Order order = new Order
            {
                name = name,
                adress = adress,
                phone = phone,
                price = price,
                date = date,
                orderedDish = orderedDish
            };
            collection.InsertOne(order);
        }

        public List<Order> OrderGetAll()
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("orders");
            List<Order> orders = collection.Find<Order>(new BsonDocument()).ToList<Order>();
            return orders;
        }

        public void OrderDelete(ObjectId id)
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("orders");
            collection.DeleteOne(x => x.id == id);
        }

        public Order OrderGet(ObjectId id)
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("orders");
            Order order = collection.Find<Order>(x => x.id == id).SingleOrDefault<Order>();
            return order;
        }

        #endregion

        #region Deliverer

        public void DelivererCreate(string name)
        {
            IMongoCollection<Deliverer> collection = database.GetCollection<Deliverer>("deliverers");
            Deliverer deliverer = new Deliverer
            {
                name = name,
                ordersIds = new List<ObjectId>(),
                available = true
            };
            collection.InsertOne(deliverer);
        }

        public Deliverer DelivererGet(ObjectId id)
        {
            IMongoCollection<Deliverer> collection = database.GetCollection<Deliverer>("deliverers");
            Deliverer deliverer = collection.Find<Deliverer>(x => x.id == id).SingleOrDefault<Deliverer>();
            return deliverer;
        }

        public List<Deliverer> DelivererGetAll()
        {
            IMongoCollection<Deliverer> collection = database.GetCollection<Deliverer>("deliverers");
            List<Deliverer> deliverers = collection.Find<Deliverer>(new BsonDocument()).ToList<Deliverer>();
            foreach(Deliverer deliverer in deliverers)
            {
                deliverer.orders = new List<Order>();
                foreach(ObjectId id in deliverer.ordersIds)
                {
                    Order oreder = OrderGet(id);
                    if (id != null)
                        deliverer.orders.Add(oreder);
                }
            }
            return deliverers;
        }

        public void DelivererAssignDeliverer(ObjectId order, ObjectId deliverer)
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("orders");
            Deliverer _deliverer = DelivererGet(deliverer);
            UpdateDefinition<Order> update = Builders<Order>.Update.Set("deliverer", _deliverer);
            collection.FindOneAndUpdate(x => x.id==order, update);
        }

        public void DelivererSetAvailable(ObjectId deliverer,bool available)
        {
            IMongoCollection<Deliverer> collection = database.GetCollection<Deliverer>("deliverers");
            UpdateDefinition<Deliverer> update = Builders<Deliverer>.Update.Set("available", available);
            collection.FindOneAndUpdate(x => x.id == deliverer, update);
        }

        public void DelivererDeliver(ObjectId order, ObjectId deliverer)
        {
            Order horder = OrderGet(order);
            OrderDelete(deliverer);

            IMongoCollection<Deliverer> collection = database.GetCollection<Deliverer>("deliverers");
            UpdateDefinition<Deliverer> update = Builders<Deliverer>.Update.PullFilter("orders", Builders<Dish>.Filter.Eq("_id", order));
            collection.FindOneAndUpdate(x => x.id == deliverer, update);
            HistoryOrderCreate(horder);
        }

        #endregion

        #region History

        public void HistoryOrderCreate(Order order)
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("history");
            collection.InsertOne(order);
        }

        public List<Order> HistoryOrderGetAll()
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("history");
            List<Order> orders = collection.Find<Order>(new BsonDocument()).ToList<Order>();
            return orders;
        }

        public void HistoryOrderDelete(ObjectId id)
        {
            IMongoCollection<Order> collection = database.GetCollection<Order>("history");
            collection.DeleteOne(x => x.id == id);
        }

        #endregion

        public void DeleteAllData()
        {
            database.DropCollection("categories");
            database.DropCollection("orders");
            database.DropCollection("deliverers");
            database.DropCollection("history");
        }
    }
}