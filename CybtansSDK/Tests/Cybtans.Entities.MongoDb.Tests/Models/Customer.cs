using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Entities.MongoDb.Tests.Models
{
    public class Customer: IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int State { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public GeoJson2DCoordinates Coordinates { get; set; }
        public int Scoring { get; set; }
        public bool Enabled { get; set; }

    }
}
