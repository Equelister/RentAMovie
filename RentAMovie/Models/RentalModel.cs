using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentAMovie.Models
{
    public class RentalModel
    { 
        [BsonId]
        public ObjectId ID { get; set; }
        public ObjectId Client { get; set; }
        public ObjectId Movie { get; set; }

        public DateTime RentDate { get; set; }
        public DateTime DateOfIntendedReturn { get; set; }
        public DateTime? DateOfRealReturn { get; set; }



/*        public virtual UserModel UserM { get; set; }
        public virtual MovieModel MovieM { get; set; }*/

        public RentalModel()
        {

        }
        public RentalModel(ObjectId userID, ObjectId movieID)
        {
            var dateNow = DateTime.Now;
            Client = userID;
            Movie = movieID;
            RentDate = dateNow;
            DateOfIntendedReturn = dateNow.AddDays(14);
            DateOfRealReturn = null;
        }
    }
}
