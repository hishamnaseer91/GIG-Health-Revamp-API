using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MemberPortalGICWebApi.Controllers
{
    public class AccountController : ApiController
    {
      

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {

           string h= Common.Encrypt("123456");
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           // var  result = await _repo.RegisterUser(userModel);

           // IHttpActionResult errorResult = GetErrorResult(result);

            //if (errorResult != null)
            //{
            //    return errorResult;
            //}

            return Ok();
        }

       

        private IHttpActionResult GetErrorResult(int result)
        {
            if (result == 0)
            {
                return InternalServerError();
            }

            if (result==-1)
            {
                
                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
