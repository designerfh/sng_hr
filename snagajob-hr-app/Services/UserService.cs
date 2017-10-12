using MongoDB.Driver;
using MongoDB.Driver.Linq;
using snagajob_hr_app.Classes;
using snagajob_hr_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snagajob_hr_app.Services
{
    public class UserService
    {

        public async Task<User> NewUser(User user)
        {
            var collection = MongoContext.UsersCollection();
            await collection.InsertOneAsync(user);

            user.Password = string.Empty;

            return user;
        }

        public async Task<List<KeyValuePair<string, string>>> Login(string username, string password)
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

            var collection = MongoContext.UsersCollection();
            IMongoQueryable<User> query = collection.AsQueryable();
            var foundUser = await query.Where(x => x.Name.ToLower() == username).FirstOrDefaultAsync();

            if (foundUser != null && foundUser.Password == password)
            {
                data.Add(new KeyValuePair<string, string>("username", foundUser.Name));
                data.Add(new KeyValuePair<string, string>("userId", foundUser._id.ToString()));
                data.Add(new KeyValuePair<string, string>("role", foundUser.Role));
            }
            else
            {
                throw new UnauthorizedAccessException();
            }

            return data;
        }
    }
}