using System;
using System.Collections.Generic;
using Mongo = MongoDB.Bson.Serialization.Attributes;

namespace SampleApi.Storage
{
    public class Alert
    {
        [Mongo.BsonId]
        [Mongo.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String MongoID { get; set; }

        private int? _ID;
        public int ID 
        { 
            get 
            { 
                if(!_ID.HasValue)
                    _ID = String.IsNullOrEmpty(this.MongoID) ? 0 : this.MongoID.GetHashCode();

                return _ID.Value;
            }
            set { _ID = value; }
        }

        public String Title { get; set; }

        public String Description { get; set; }

        public String Severity { get; set; }

        public IEnumerable<IPAdress> Adresses { get; set; }
    }

    public class IPAdress
    {
        public String Url { get; set; }

        public Boolean Blacklisted { get; set; }

        public Source SourceType { get; set; }
    }

    public enum Source : short
    {
        Any = 0,
        Internal = 1,
        External = 2
    }
}
