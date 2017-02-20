using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Pizzeria.Models.Mongo.Models
{
    public class Restaurant
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId id { get; set; }
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("address")]
        public string address { get; set; }
        [BsonElement("coordinates")]
        public double[] coordinates { get; set; }
    }
}