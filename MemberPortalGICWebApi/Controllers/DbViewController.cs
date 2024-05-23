using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.DBView;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemberPortalGICWebApi.Controllers
{
    [RoutePrefix("api/DbView")]
    public class DbViewController : ApiController
    {

        DbViewDAL repo = new DbViewDAL();
        [Route("ViewDBRequest")]
        [HttpGet]
        public IHttpActionResult GetDbView()
        {
            var list = repo.GetAllusers();
            if (list != null)     
            {
                //foreach (var item in list)
                //{
                //    item.password = Common.Decrypt(item.password);
                //}
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "GetDbView list Success",
                    DescriptionArabic = "GetDbView list Success",
                    Data = list
                };
                return Ok(code);
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-02",
                    Type = "Get",
                    Description = "GetDbView list No Record Found",
                    DescriptionArabic = "GetDbView list No Record Found"

                };
                return Ok(code);
            }
        }


        [Route("Psw")]
        [HttpGet]
        public IHttpActionResult GetPswView(ChangePasswordModel model)
        {



            APIResponceCodes code = new APIResponceCodes()
            {
                Code = "CD-02",
                Type = "Get",
                Description = "Decrypted Pasword",
                DescriptionArabic = "Encrypted Pasword",
                Data = Common.Decrypt(model.Password)
                };
                return Ok(code);
          
        }
    }
}
