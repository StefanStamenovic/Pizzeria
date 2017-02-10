using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models.Mongo.Models
{
    public class Supplement
    {

        [BsonId]
        public ObjectId id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("price")]
        public int price { get; set; }
    }
}