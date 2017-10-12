using MongoDB.Driver;
using snagajob_hr_app.Models;
using snagajob_hr_app.Services;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace snagajob_hr_app.Controllers
{
    [RoutePrefix("v1/Result")]
    public class ResultController : ApiController
    {
        private ResultService resultService;

        public ResultController()
        {
            resultService = new ResultService();
        }

        //get passed results
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetResults()
        {
            try
            {
                var results = await resultService.GetPassedApplicationResults();
                return Ok(results);
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }

        
        [Route("{userId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetResult([FromUri] string userId)
        {
            try
            {
                var result = await resultService.GetApplicationResultByUserId(userId);
                return Ok(result);
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }

        //create/assign result
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AssignResult([FromBody] JobApplicationResult newResult)
        {
            try
            {
                await resultService.AssignResult(newResult);
                return Ok("OK");
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }

        //submit/update result
        [Route("")]
        [HttpPut]
        public async Task<IHttpActionResult> SubmitResult([FromBody] JobApplicationResult exstResult)
        {
            try
            {
                var modifiedCount = await resultService.SubmitResult(exstResult);
                return Ok(modifiedCount);
            }
            catch (InvalidDataException idex)
            {
                return BadRequest("This application has already been submitted.");
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
