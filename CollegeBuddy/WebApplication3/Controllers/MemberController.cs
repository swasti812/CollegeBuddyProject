using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using WebApplication3.Models;
using System.Web.Security;
using System.Security.Claims;

//using models;

namespace WebApplication3.Controllers
{
    public class MemberController : ApiController
    {

        [HttpPost]
        [Route("api/Member/Signup")]
        public HttpResponseMessage SignUp([FromBody] Detail detail)
        {
            try
            {
                var x = MemberDetails.IsExist(detail.MobileNo);
                if (x == false)
                {
                    detail.Password = Crypto.Hash(detail.Password);
                    var context = new CollegeBuddyEntities();
                    new MemberDetails().otpfunc(detail);
                    context.Details.Add(detail);
                    context.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, detail);
                    message.Headers.Location = new Uri(Request.RequestUri + detail.ID.ToString());
                    return message;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict, "Account already exists");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }


        }

      
        [Route("api/Member/Verify/{ID}")]
        [HttpPost]
        public HttpResponseMessage Verify([FromBody]string otp,[FromUri] int ID)
        {
            try
            {   

                var context = new CollegeBuddyEntities();
                var obj = context.Details.SingleOrDefault(x=>x.ID==ID);
                var time = DateTime.Now;
                TimeSpan s = (TimeSpan) (time - obj.datetime);
                if (obj.AuthCode == otp) 
                {   if (s.TotalMinutes < 2)
                    {
                        obj.IsVerified = true;
                        context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Verified");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "OTP Expired");
                    }
                }
                else
                {

                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Invalid OTP");

                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }

        [HttpPost]
        [Route("api/Member/Login")]
        public HttpResponseMessage Login([FromBody] Detail detail)
        {
            try
            {
                var context = new CollegeBuddyEntities();
                var xyz = Crypto.Hash(detail.Password);
                Detail CurrentUser = context.Details.SingleOrDefault(x => x.MobileNo == detail.MobileNo && x.Password == xyz);
                if (CurrentUser == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Username/Password is incorrect");
                }
                else
                {
                    if (CurrentUser.IsVerified == false)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Verify account first");
                    }
                    else
                    {
                        TOKEN_cs obj = new TOKEN_cs();
            
                        //var a = "token:" +TokenManager.GenerateToken(CurrentUser.Name);
                        return Request.CreateResponse(HttpStatusCode.OK,TokenManager.GenerateToken(CurrentUser.MobileNo));
                        
                    }
                }

                

            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }


        }

        [HttpPost]
        [Route("api/Member/ResendOtp/{ID}")]
        public HttpResponseMessage ResendOtp([FromUri] int ID)
        {
            try
            {
                var context = new CollegeBuddyEntities();
                var obj = context.Details.SingleOrDefault(x => x.ID == ID);
                new MemberDetails().otpfunc(obj);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "OTP Sent");
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }

      
        public static string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = TokenManager.GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }



        [HttpGet]
        [Route("api/Member/Profile")]

        public HttpResponseMessage Profile([FromUri] string token)
        {
            try
            {
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                MemberDetails.YearEdit(obj);
                var query = context.Details.Where(x => x.ID == obj.ID).Select(p => new { p.ID, p.Image, p.Name, obj.Year, p.Branch, p.College }).ToList();
                MemberDetails.YearFix(obj);
                return Request.CreateResponse(HttpStatusCode.OK, query);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            } }
   

        [HttpPost]
        [Route("api/Member/EditProfile")]
     public HttpResponseMessage EditProfile([FromUri] string token,[FromBody] EditProfile obj)
        {
            try
            {
                var context = new CollegeBuddyEntities();
                var x = TokenManager.Identifytoken(token);
                if (x == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var message = MemberDetails.ProfileEdit(x.ID, obj);
                
                return Request.CreateResponse(HttpStatusCode.OK, message);

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
            
    }
}