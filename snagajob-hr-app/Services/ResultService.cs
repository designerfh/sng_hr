using MongoDB.Driver;
using MongoDB.Driver.Linq;
using snagajob_hr_app.Classes;
using snagajob_hr_app.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace snagajob_hr_app.Services
{
    public class ResultService
    {
        private IMongoCollection<JobApplicationResult> collection;

        public ResultService()
        {
            collection = MongoContext.ResultsCollection();
        }

        public async Task<List<JobApplicationResult>> GetPassedApplicationResults()
        {
            IMongoQueryable<JobApplicationResult> query = collection.AsQueryable();
            var passedResults = await query.Where(x => x.Passed == true).ToListAsync();

            return passedResults;
        }

        public async Task<JobApplicationResult> GetApplicationResultByUserId(string userId)
        {
            IMongoQueryable<JobApplicationResult> query = collection.AsQueryable();
            var result = await query.Where(x => x.User_Id == userId && (x.Complete == false || x.Complete == null)).FirstOrDefaultAsync();

            return result;
        }

        public async Task AssignResult(JobApplicationResult newResult)
        {
            await collection.InsertOneAsync(newResult);
        }

        public async Task<long> SubmitResult(JobApplicationResult exstResult)
        {
            long modifiedCount;

            if (exstResult.Complete != true)
            {
                exstResult.Validate();

                var filter = Builders<JobApplicationResult>.Filter.Eq("_id", exstResult._id);
                var result = await collection.ReplaceOneAsync(filter, exstResult);
                modifiedCount = result.ModifiedCount;
            }
            else
            {
                throw new InvalidDataException();
            }

            return modifiedCount;
        }
    }
}