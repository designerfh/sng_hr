using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using snagajob_hr_app.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace snagajob_hr_app.Models
{
    public class JobApplicationResult
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _id { get; set; }
        public string Application_Id { get; set; }
        public string ApplicationName { get; set; }
        public string User_Id { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> Passed { get; set; }
        public Nullable<bool> Complete { get; set; }
        public List<UserAnswer> Answers { get; set; }

        public void Validate()
        {
            var collection = MongoContext.ApplicationsCollection();
            IMongoQueryable<JobApplication> query = collection.AsQueryable();
            var app = query.Where(x => x._id == ObjectId.Parse(this.Application_Id)).First();

            if (this.Answers != null && this.Answers.Count == app.Questions.Count)
            {
                foreach (UserAnswer tempAnswer in this.Answers)
                {
                    var question = app.Questions.Where(x => x.QuestionText == tempAnswer.QuestionText).FirstOrDefault();
                    if (question != null)
                    {
                        tempAnswer.Correct = question.Answers.Where(x => x.AnswerText == tempAnswer.AnswerText).Select(x => x.Correct).First();
                    }
                }
                 
                this.Complete = true;
                this.Passed = !this.Answers.Where(x => x.Correct == false).Any();
            }
            else
            {
                this.Complete = false;
            }
        }
    }

    public class UserAnswer
    {
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public bool Correct { get; set; }
    }
} 