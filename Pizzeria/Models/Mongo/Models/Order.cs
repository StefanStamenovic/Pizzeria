using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models.Mongo.Models
{
    public class Order
    {
        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("adress")]
        public string adress { get; set; }

        [BsonElement("phone")]
        public string phone { get; set; }

        [BsonElement("price")]
        public string price { get; set; }

        [BsonElement("date")]
        public string date { get; set; }

        [BsonElement("location")]
        public Location location { get; set; }
    }
}