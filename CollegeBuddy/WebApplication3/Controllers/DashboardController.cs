using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class DashboardController : ApiController
    {
        [HttpGet]
        [Route("api/Dashboard/Home")]
        public HttpResponseMessage DB([FromUri] string token)
        {

            try
            {               
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var objx = from x in context.DashboardQuestions join y in context.Details on x.Number equals y.ID where x.CollegeName == obj.College select new { x.Question,x.QID,x.Datetime,x.ID,y.Image};
                //var objx = context.DashboardQuestions.SqlQuery("Select * from DashboardQuestion where CollegeName= '"+obj.College+"'");                      
                 return Request.CreateResponse(HttpStatusCode.OK, objx);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        
        [HttpPost]
        [Route("api/Dashboard/AskQuestion")]
        public HttpResponseMessage Question([FromUri] string token,[FromBody] string Ques)
        {
            try
            {
             var context = new CollegeBuddyEntities();
             var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                DashboardQuestion q = new DashboardQuestion();
            q.Question = Ques;
            q.ID = obj.Name;
            q.Datetime = DateTime.Now.Date;
            q.Number = obj.ID;
            q.CollegeName = obj.College;
            context.DashboardQuestions.Add(q);
            context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Added");
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPost]
        [Route("api/Dashboard/AddAnswer/{QID}")]
        public HttpResponseMessage Answer([FromUri]string token,[FromUri] int QID, [FromBody] string answer)
        {
            try
            {               
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                DashboardAnswer a = new DashboardAnswer();
                a.Answer = answer;
                a.QID = QID;
                a.Name = obj.Name;
                a.Upvotes = 0;
                a.identify = obj.ID;
                context.DashboardAnswers.Add(a);
                context.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Added");
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }//QID)

        [HttpGet]
        [Route("api/Dashboard/QuestionDetails/{QID}")]
        public HttpResponseMessage QuestionDetail([FromUri] string token,[FromUri] int QID)
        {
            try
            {
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var objx = context.DashboardAnswers.Where(x => x.QID == QID).Select(p => new { p.Answer,p.Name,p.Upvotes });
                return Request.CreateResponse(HttpStatusCode.OK, objx);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Dashboard/Upvote/{QID}/{AID}")]
        public HttpResponseMessage Upvote([FromUri] string token, [FromUri] int AID,[FromUri] int QID)
        {

            var context = new CollegeBuddyEntities();
            var obj = TokenManager.Identifytoken(token);
            if (obj == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
            var objx = context.DashboardAnswers.SingleOrDefault(x => x.AID == AID);
                objx.Upvotes = objx.Upvotes + 1;
                context.SaveChanges();
          
           
                return Request.CreateResponse(HttpStatusCode.OK, objx.Upvotes);
     
           
        }
       
            
            
            }

}
