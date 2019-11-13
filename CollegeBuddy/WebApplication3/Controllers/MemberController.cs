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
                detail.Password = Crypto.Hash(detail.Password);
                var context = new CollegeBuddyEntities();
                new  MemberDetails().otpfunc(detail);
                context.Details.Add(detail);
                context.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, detail);
                message.Headers.Location = new Uri(Request.RequestUri + detail.ID.ToString());
                return message;

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
                        return Request.CreateResponse(HttpStatusCode.OK, "Verified");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "OTP Expired");
                    }
                }
                else
                {

                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Recheck otp");

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
                Detail CurrentUser = context.Details.FirstOrDefault(x => x.MobileNo == detail.MobileNo && x.Password == xyz);
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
                        return Request.CreateResponse(HttpStatusCode.OK,TokenManager.GenerateToken(CurrentUser.Name));
                        
                    }
                }

                //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Username/Password is incorrect");

            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }


        }

        [HttpPost]
        [Route("api/Member/ResendOtp/{ID}")]
        public System.Web.Http.Results.RedirectToRouteResult ResendOtp([FromUri] int ID)
        {
            
                var context = new CollegeBuddyEntities();
                var obj = context.Details.SingleOrDefault(x => x.ID == ID);
                new MemberDetails().otpfunc(obj);
                context.SaveChanges();
                return RedirectToRoute("Verify",ID);
               
               
        }

        //[HttpGet]
        //[Route("api/Member/Validate")]
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

        [HttpPost]
        [Route("api/Member/ImageUpload")]
        public HttpResponseMessage ImageUpload([FromUri] string token,[FromBody]string Path)
        {
            try
            {
                var user = MemberController.ValidateToken(token);
                var context = new CollegeBuddyEntities();
                var obj = context.Details.SingleOrDefault(x => x.Name == user);
                obj.Image = Path;
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Added");

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }

        [HttpGet]
        [Route("api/Member/Profile")]

        public HttpResponseMessage Profile([FromUri] string token)
        {
            var user = MemberController.ValidateToken(token);
            var context = new CollegeBuddyEntities();
            var obj = context.Details.FirstOrDefault(xa => xa.Name == user);
            var x= context.Details.SqlQuery("Select * from Details where ID=" + obj.ID);
            return Request.CreateResponse(HttpStatusCode.OK, x);

        }
    }
}