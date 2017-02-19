using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models.Mongo.Models
{
    public class Category
    {

        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId id { get; set;}

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("dishes")]
        public List<Dish> dishes { get; set; }

        [BsonElement("supplements")]
        public List<Supplement> supplements { get; set; }
    }
}