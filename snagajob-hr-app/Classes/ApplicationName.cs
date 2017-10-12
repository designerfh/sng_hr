using MongoDB.Bson;

namespace snagajob_hr_app.Classes
{
    public class ApplicationName
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}