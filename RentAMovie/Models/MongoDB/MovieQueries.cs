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

        internal static Task<bool> CheckIsMovieRented(ObjectId iD)
        {
            throw new NotImplementedException();
        }

        internal static Task<bool> CheckIsMovieRentedChangeIfYes(ObjectId id)
        {
            throw new NotImplementedException();
        }

        internal static async Task<bool> CheckIsMovieRentedChangeIfNot(ObjectId id)
        {
            var condition = Builders<MovieModel>.Filter.Eq(p => p.ID, id);
            var fields = Builders<MovieModel>.Projection.Include(p => p.IsRented);
            var result = await _moviesColl.Find(condition).Project<MovieModel>(fields).SingleAsync();
            if(result.IsRented)
            {
                return result.IsRented;
            }else
            {
                await ChangeIsRentedFieldTo(id, true);
                return result.IsRented;
            }
        }
        internal static async Task ChangeIsRentedFieldTo(ObjectId id, bool setFieldAs)
        {
            var condition = Builders<MovieModel>.Filter.Eq(p => p.ID, id);
            var fields = Builders<MovieModel>.Update.Set(p => p.IsRented, setFieldAs);
            var result = await _moviesColl.UpdateOneAsync(condition, fields);
            /*
                        var filter = Builders<TempAgenda>.Filter.Eq(x => x.AgendaId, agendaId);
                        var update = Builders<TempAgenda>.Update.Set(x => x.Items.Single(p => p.Id.Equals(itemId)).Title, title);
                        var result = _collection.UpdateOneAsync(filter, update).Result;*/
        }
    }
}
