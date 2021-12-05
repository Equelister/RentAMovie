using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentAMovie.Models.MongoDB
{
    class DBConnection
    {
        protected static MongoClient _client = new MongoClient("mongodb://localhost:27017");
        protected static IMongoDatabase _database = _client.GetDatabase("rentamovie");
    }
}
