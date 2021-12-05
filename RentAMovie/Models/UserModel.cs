using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentAMovie.Models
{
    public class UserModel
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime RegisterDate  { get; set; }
        public bool IsAdmin  { get; set; }
        public int RentalsCount { get; set; }


        public UserModel()
        {

        }
    }
}
