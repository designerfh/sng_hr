using MongoDB.Bson;
using Newtonsoft.Json;
using snagajob_hr_app.Classes;

namespace snagajob_hr_app.Models
{
    public class User
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}