using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models.Mongo.Models
{
    public class OrderedDish
    {
        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("dish")]
        public Dish dish { get; set; }

        [BsonElement("quantity")]
        public int quantity { get; set; }

        [BsonElement("supplements")]
        public List<Supplement> supplements { get; set; }

        [BsonElement("price")]
        public int price { get; set; }
    }
}