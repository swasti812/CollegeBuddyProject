using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ContactsController : ApiController
    {
        [HttpGet]
        [Route("api/Contact/List")]
        public HttpResponseMessage ListOfContacts([FromUri] string token)
        {
            try
            {
                var user = MemberController.ValidateToken(token);
                //DashboardDetails obj;

                var context = new CollegeBuddyEntities();
                var obj = context.Details.FirstOrDefault(x => x.Name == user);
                var objx = context.Details.Where(x=>x.College==obj.College).Select(p=>new { p.Name,p.Image,p.Branch,p.Year});
                return Request.CreateResponse(HttpStatusCode.OK, objx);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
