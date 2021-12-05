using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentAMovie.Models
{
    public class MovieModel
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }
        public int YearOfRelease { get; set; }
        public List<string> Actors { get; set; }
        public double Rate { get; set; }
        public string Summary { get; set; }
        public DateTime InsertDate { get; set; }
        public bool IsRented { get; set; }


        public MovieModel()
        {

        }
    }
}
