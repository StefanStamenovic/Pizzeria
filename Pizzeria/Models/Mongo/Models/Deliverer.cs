using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models.Mongo.Models
{
    public class Deliverer
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("available")]
        public bool available { get; set; }

        [BsonElement("orders")]
        public List<ObjectId> ordersIds { get; set; }

        [BsonIgnore]
        public List<Order> orders { get; set; }
    }
}