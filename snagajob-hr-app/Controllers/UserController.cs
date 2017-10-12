using MongoDB.Driver;
using snagajob_hr_app.Models;
using snagajob_hr_app.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace snagajob_hr_app.Controllers
{
    [RoutePrefix("v1/User")]
    public class UserController : ApiController
    {
                private UserService userService;
        
        public UserController()
        {
            userService = new UserService();
        }

        //create new user
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser([FromBody] User user)
        {
            try {
                var newUser = await userService.NewUser(user);
                return Ok(newUser);
            }
            catch (MongoException ex)
            {
                return InternalServerError(ex);
            }
        }

        //login user
        [Route("Login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login()
        {
            IEnumerable<string> headerValues;
            if (Request.Headers.TryGetValues("Authorization", out headerValues) == false)
            {
                return Unauthorized();
            }
            else
            {
                var auth = headerValues.ElementAt(0);
                if (auth.StartsWith("Basic"))
                {
                    var base64str = auth.Substring(6);
                    var creds = Encoding.ASCII.GetString(Convert.FromBase64String(base64str)).Split(':');

                    var username = creds[0].ToLower();
                    var password = creds[1];

                    try
                    {
                        var data = await userService.Login(username, password);
                        return Ok(data);
                    }
                    catch (UnauthorizedAccessException uaex)
                    {
                        return Unauthorized();
                    }
                    catch (MongoException ex)
                    {
                        return InternalServerError(ex);
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
        }
    }
}
