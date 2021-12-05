using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentAMovie.Models.MongoDB
{
    public static class RentalQueries
    {
        private static MongoClient _client = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase _database = _client.GetDatabase("rentamovie");
        private static IMongoCollection<RentalModel> _usersColl = _database.GetCollection<RentalModel>("rentals");

        public async static Task<RentalModel> FindRentalByID(BsonObjectId id)
        {
            // var xd = await _usersColl.Find(new BsonDocument()).ToListAsync();
            return await _usersColl.Find(x => x.ID.Equals(id)).SingleAsync();
        }

        public async static Task<List<RentalModel>> FindAllRentals()
        {
            try
            {
                return await _usersColl.Find(new BsonDocument()).ToListAsync();
            }catch(Exception efefe)
            {
                return null;
            }
            //return await _usersColl.Find(_ => true).ToListAsync();
        }
    }
}
