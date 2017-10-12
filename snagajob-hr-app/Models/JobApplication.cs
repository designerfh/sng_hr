using MongoDB.Bson;
using Newtonsoft.Json;
using snagajob_hr_app.Classes;
using System.Collections.Generic;

namespace snagajob_hr_app.Models
{
    public class JobApplication
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _id { get; set; }

        public string Name { get; set; }

        public List<Question> Questions { get; set; }

    }

    public class Question
    {
        public string QuestionText { get; set; }

        public List<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public string AnswerText { get; set; }

        public bool Correct { get; set; }
    }
}