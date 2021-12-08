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

        internal static async Task<int> CheckUserRentalLimits(ObjectId id)
        {
            var condition = Builders<UserModel>.Filter.Eq(p => p.ID, id);
            var fields = Builders<UserModel>.Projection.Include(p => p.RentalsCount);
            var result = await _usersColl.Find(condition).Project<UserModel>(fields).SingleAsync();
            return result.RentalsCount;
        }

        internal static async Task IncrementUserRentalLimitsByOne(ObjectId id)
        {
            int setFieldAs = await CheckUserRentalLimits(id);

            var condition = Builders<UserModel>.Filter.Eq(p => p.ID, id);
            var fields = Builders<UserModel>.Update.Set(p => p.RentalsCount, setFieldAs+1);
            var result = await _usersColl.UpdateOneAsync(condition, fields);
        }

        internal static async Task<bool> UpdateUser(UserModel user)
        {
            var filter = Builders<UserModel>.Filter.Eq(s => s.ID, user.ID);
            ReplaceOneResult result;
            try
            {
                 result = await _usersColl.ReplaceOneAsync(filter, user);
            }catch(Exception ek)
            {
                Console.WriteLine(ek.ToString());
                return false;
            }
            long count = 0;
            try
            {
                count = result.ModifiedCount;                
            }catch(Exception e)
            {
                return false;
            }
            if (count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static async Task<long> DeleteUser(UserModel user)
        {
            var filter = Builders<UserModel>.Filter.Eq(s => s.ID, user.ID);
            var result = await _usersColl.DeleteOneAsync(filter);
            return result.DeletedCount;
        }

        internal static async Task InsertUser(UserModel user)
        {
            //var filter = Builders<UserModel>.Filter.Eq(s => s.ID, user.ID);
            await _usersColl.InsertOneAsync(user);            
        }
    }
}
