using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
            /*Fetches all the users from a particular college which the user belongs to*/
            try
            {
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Invalid Token" );
                
                var objx = context.Details.Where(x=>x.College==obj.College).Select(p=>new { p.Name,p.Image,p.Branch,p.Year});
                return Request.CreateResponse(HttpStatusCode.OK, objx);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpGet]
        [Route("api/Contact/SubjectList")]
        public HttpResponseMessage ListOfSubjects([FromUri] string token)
        {
            try
            {
               /*Returns a list of matching subjects corresponding to users branch and year*/
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var y = context.BranchTables.SingleOrDefault(x => x.Branch == obj.Branch);
                             
               
                   var listsubject = context.SubjectTables.Where(x => x.Year.ToString() == obj.Year && x.BID.ToString() == y.BID.ToString()).Select(x=> new { x.Subject,x.Key}).ToList();
                             
               return Request.CreateResponse(HttpStatusCode.OK, listsubject);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/Contact/QuestionTab")]
        public HttpResponseMessage QuestionTab([FromUri]string token)
        {
            /*Returns all the questions asked by user for easy access to solutions*/
            try
            {
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var listquestion = from x in context.DashboardQuestions join y in context.Details on x.Number equals y.ID where x.Number == obj.ID select new { x.ID, y.Name, x.Question, x.QID, x.Datetime };
                //var listquestion = context.DashboardQuestions.Where(x => x.Number == obj.ID).Select(p => new { p.Question,p.QID,p.Datetime }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK,listquestion );
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/Contact/AnswerTab")]
        public HttpResponseMessage AnswerTab([FromUri]string token)
        {
            try
            {
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var answer = context.DashboardAnswers.Where(x => x.identify == obj.ID).Select(p => new { p.Answer }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, answer);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Contacts/ImageUpload")]
        
        public HttpResponseMessage UserImage([FromUri] string token)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {     /*Profile Image upload for user*/                      
               
                var httpRequest = HttpContext.Current.Request;
                
                foreach (string file in httpRequest.Files)
                {

                    var context = new CollegeBuddyEntities();
                    var obj = TokenManager.Identifytoken(token);
                    if (obj == null)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                    Detail user = new Detail();
                    user = context.Details.SingleOrDefault(x => x.ID == obj.ID);
                    var postedFile = httpRequest.Files[file];
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    var filePath = HttpContext.Current.Server.MapPath("~/Images/" + postedFile.FileName );
                    
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {                      
                        List<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                       
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {                   

                            postedFile.SaveAs(filePath);
                         }


                    } 
                    
                    var message1 = string.Format("Image Updated Successfully.");
                     user.Image = @"\Images\" + postedFile.FileName;
                    context.SaveChanges();

                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                   
                }
               

                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);

            }
            catch (Exception ex)
            {
                
                return Request.CreateResponse(HttpStatusCode.NotFound, ex);
            }
        }

        



    }


}
