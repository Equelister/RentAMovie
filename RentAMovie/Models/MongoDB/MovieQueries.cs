using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentAMovie.Models.MongoDB
{
    public static class MovieQueries
    {
        private static MongoClient _client = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase _database = _client.GetDatabase("rentamovie");
        private static IMongoCollection<MovieModel> _moviesColl = _database.GetCollection<MovieModel>("movies");
        //private static IMongoCollection<MovieModel> _moviesColl = _database.GetCollection<MovieModel>("movies");

        public async static Task<MovieModel> FindMovieByID(BsonObjectId id)
        {
            // var xd = await _usersColl.Find(new BsonDocument()).ToListAsync();
            return await _moviesColl.Find(x => x.ID.Equals(id)).SingleAsync();
        }

        public async static Task<List<MovieModel>> FindAllMovies()
        {
            try
            {
                return await _moviesColl.Find(new BsonDocument()).ToListAsync();
            }catch(Exception eeeee)
            {
                string wada = ";";
            }
            return null;
        }
    }
}
