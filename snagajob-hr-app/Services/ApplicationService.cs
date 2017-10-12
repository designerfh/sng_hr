using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using snagajob_hr_app.Classes;
using snagajob_hr_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snagajob_hr_app.Services
{
    public class ApplicationService
    {
        private IMongoCollection<JobApplication> collection;


        public ApplicationService()
        {
            collection = MongoContext.ApplicationsCollection();
        }

        public async Task<List<ApplicationName>> GetApplicationNames()
        {
            IMongoQueryable<JobApplication> query = collection.AsQueryable();
            var jobApps = await query.Select(x => new ApplicationName { Id = x._id, Name = x.Name }).ToListAsync();

            return jobApps;
        }

        public async Task<JobApplication> GetApplicationById(string id, string role)
        {
            IMongoQueryable<JobApplication> query = collection.AsQueryable();
            var result = await query.Where(x => x._id == ObjectId.Parse(id)).FirstAsync();

            if (role == "applicant")
            {
                foreach (Answer answer in result.Questions.SelectMany(x => x.Answers))
                {
                    answer.Correct = false;
                }
            }

            return result;
        }

        public async Task CreateApplication(JobApplication newApplication)
        {
            await collection.InsertOneAsync(newApplication);
        }

        public async Task<long> UpdateApplication(JobApplication exstApplication)
        {
            long modifiedCount;

            var result = await collection.ReplaceOneAsync(x => x._id == exstApplication._id, exstApplication);
            modifiedCount = result.ModifiedCount;

            return modifiedCount;
        }

        public async Task DeleteApplication(string id)
        {
            var filter = Builders<JobApplication>.Filter.Eq("_id", ObjectId.Parse(id));
            await collection.FindOneAndDeleteAsync(filter);
        }
    }


}