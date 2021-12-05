using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentAMovie.Models.MongoDB
{
    public static class UserQueries
    {
        private static MongoClient _client = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase _database = _client.GetDatabase("rentamovie");
        private static IMongoCollection<UserModel> _usersColl = _database.GetCollection<UserModel>("clients");

        public static void Initconnection()
        {

        }

        public async static Task<UserModel> FindUserByLogin(BsonString login)
        {
            // var xd = await _usersColl.Find(new BsonDocument()).ToListAsync();
            return await _usersColl.Find(x => x.Login.Equals(login)).SingleAsync();
        }

        public async static Task<List<UserModel>> FindAllUsers()
        {
            return await _usersColl.Find(new BsonDocument()).ToListAsync();
            //return await _usersColl.Find(_ => true).ToListAsync();
        }
    }
}
