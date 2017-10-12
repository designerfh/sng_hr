using MongoDB.Driver;
using snagajob_hr_app.Models;
using snagajob_hr_app.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace snagajob_hr_app.Controllers
{
    [RoutePrefix("v1/Application")]
    public class ApplicationController : ApiController
    {

        private ApplicationService applicationService;

        public ApplicationController()
        {
            applicationService = new ApplicationService();
        }

        //get application names
        [Route("Names")]
        [HttpGet]
        public async Task<IHttpActionResult> GetApplicationNames()
        {
            try
            {
                var jobApps = await applicationService.GetApplicationNames();
                return Ok(jobApps);
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }

        //get application by id
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetApplication([FromUri] string id, [FromUri] string role)
        {
            try
            {
                var result = await applicationService.GetApplicationById(id, role);
                return Ok(result);
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }

        //create application
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateApplication([FromBody] JobApplication newApplication)
        {
            try
            {
                await applicationService.CreateApplication(newApplication);
                return Ok("OK");
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }

        //update application
        [Route("")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateApplication([FromBody] JobApplication exstApplication)
        {
            try
            {
                var modifiedCount = await applicationService.UpdateApplication(exstApplication);
                return Ok(modifiedCount);
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }

        //delete application by id
        [Route("")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteApplication([FromUri] string id)
        {
            try
            {
                await applicationService.DeleteApplication(id);
                return Ok("OK");
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
