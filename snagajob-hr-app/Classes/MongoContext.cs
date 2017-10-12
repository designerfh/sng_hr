using MongoDB.Driver;
using snagajob_hr_app.Models;
using System.Configuration;

namespace snagajob_hr_app.Classes
{
    public class MongoContext
    {
        private static MongoClient _context;
        private static IMongoDatabase _db;

        public static MongoClient Instance()
        {
            if (_context == null)
            {
                _context = new MongoClient(GlobalVars.MongoConnection());
                _db = _context.GetDatabase(GlobalVars.MongoHrDb());
            }
            return _context;
        }

        public static IMongoDatabase HR_Database()
        {
            if (_context == null)
            {
                MongoContext.Instance();
            }
            return _db;
        }

        public static IMongoCollection<JobApplicationResult> ResultsCollection()
        {
            if (_context == null)
            {
                MongoContext.Instance();
            }

            return _db.GetCollection<JobApplicationResult>("results");
        }

        public static IMongoCollection<JobApplication> ApplicationsCollection()
        {
            if (_context == null)
            {
                MongoContext.Instance();
            }

            return _db.GetCollection<JobApplication>("applications");
        }

        public static IMongoCollection<User> UsersCollection()
        {
            if (_context == null)
            {
                MongoContext.Instance();
            }

            return _db.GetCollection<User>("users");
        }

    }
}