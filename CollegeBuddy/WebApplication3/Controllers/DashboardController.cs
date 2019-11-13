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
        public System.Data.Entity.Infrastructure.DbSqlQuery<DashboardQuestion> DB([FromUri] string token)
        {


            var user = MemberController.ValidateToken(token);
            //DashboardDetails obj;
            var context = new CollegeBuddyEntities();
            var obj = context.Details.FirstOrDefault(x => x.Name == user);

            //var context = new CollegeBuddyEntities();
            var objx = context.DashboardQuestions.SqlQuery("Select * from DashboardQuestion ");
            //var a = "hello";
            return objx;//obj=context.

        }

        
        [HttpPost]
        [Route("api/Dashboard/AskQuestion")]
        public HttpResponseMessage Question([FromUri] string token,[FromBody] string Ques)
        {
            try
            {
                
            var user = MemberController.ValidateToken(token);
            var context = new CollegeBuddyEntities();
            var obj = context.Details.FirstOrDefault(x=>x.Name==user);
            DashboardQuestion q = new DashboardQuestion();
            q.Question = Ques;
            q.ID = obj.Name;
            q.Datetime = DateTime.Now;
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
                var user = MemberController.ValidateToken(token);
                var context = new CollegeBuddyEntities();
                var obj = context.Details.FirstOrDefault(x => x.Name == user);
                DashboardAnswer a = new DashboardAnswer();
                a.Answer = answer;
                a.QID = QID;
                a.Name = user;
                a.Upvotes = 0;
                //a.ID = user;
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
                var user = MemberController.ValidateToken(token);
                var context = new CollegeBuddyEntities();
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
        public System.Web.Http.Results.RedirectToRouteResult Upvote([FromUri] string token, [FromUri] int AID,[FromUri] int QID)
        {
           
                var user = MemberController.ValidateToken(token);
                var context = new CollegeBuddyEntities();
                var objx = context.DashboardAnswers.SingleOrDefault(x => x.AID == AID);
                objx.Upvotes = objx.Upvotes + 1;
                context.SaveChanges();
          
            return RedirectToRoute("api/Dashboard/QuestionDetails/{QID}",new { QID=QID });
                //return Request.CreateResponse(HttpStatusCode.OK, objx.Upvotes);
     
           
        }
        //[HttpPost]
        //[Route("api/Dashboard/Comment/{AID}")]

        //public HttpResponseMessage AddComment ([FromUri] string token,[FromBody] string comment, [FromUri] int AID)
        //{
        //    try
        //    {


        //    }
        //    catch(Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}
            
            
            }

}
