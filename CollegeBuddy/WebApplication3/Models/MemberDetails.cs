using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class MemberDetails
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string College { get; set; }
        [Required]
        public string Branch { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Please enter a valid Phone Number")]
        public string MobileNo { get; set; }

        public string Image { get; set; }
        [Required]
        public string Password { get; set; }

        public string GenerateRandomOTP(Detail detail)

        {
            string[] s = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            int OTPLength = 4;

            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < OTPLength; i++)

            {

                int p = rand.Next(0, s.Length);

                sTempChars = s[rand.Next(0, s.Length)];

                sOTP += sTempChars;

            }
            var OTPTIME = DateTime.Now;
            detail.datetime = OTPTIME;
            return sOTP;


        }

        public void otpfunc(Detail detail)
        {
            var otp = new MemberDetails().GenerateRandomOTP(detail);
            var abc = new SmsService().Mssg(detail, otp);
            new SmsService().Messages(abc);
            detail.AuthCode = otp;
            detail.IsVerified = false;
        }

        public static void YearEdit(Detail obj)
        {
            var context = new CollegeBuddyEntities();
            var x = obj.Year;
            if (x == "1")
                x = x + "st";
            if (x == "2")
                x = x + "nd";
            if (x == "3")
                x = x + "rd";
            if (x == "4")
                x = x + "th";

            obj.Year = x;
            context.SaveChanges();


        }
        public static void YearFix(Detail obj)
        {
            var context = new CollegeBuddyEntities();
            var x = obj.Year;
            if (x == "1st")
                x = "1";
            if (x == "2nd")
                x = "2";
            if (x == "3rd")
                x = "3";
            if (x == "4th")
                x = "4";

            obj.Year = x;
            context.SaveChanges();


        }

        public static string ProfileEdit(int ID,EditProfile editobj)
        {
            var context = new CollegeBuddyEntities();
            var obj = context.Details.SingleOrDefault(x => x.ID == ID);
            if (obj.Password == Crypto.Hash(editobj.OldPassword))
            {
                
                obj.Password = Crypto.Hash(editobj.Password);
                obj.Branch = editobj.Branch;
                obj.Year = editobj.Year;
                context.SaveChanges();
                return ("Succesfully edited");
            }
            else
            {
                return ("Recheck Password");
            }
            
        }

        public static bool IsExist(string MobileNo)
        {
            var context = new CollegeBuddyEntities();
            bool value = false;
            var account = context.Details.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
            if (account != null)
                value = true;
            return value;
        }
        
    }
    
    public class SmsService : IIdentityMessageService
    {
        public IdentityMessage Mssg(Detail obj,string otp)
        {
            IdentityMessage message = new IdentityMessage();
        
            message.Body = "Your OTP is" + otp;
            message.Destination = obj.MobileNo;
            return message;

        }
        public void Messages(IdentityMessage message)
        {
            var SMSAccountIdentification = "ACfc89f37832e6c021522da103be490f92";
            //var SMSAccountPassword = "swastitiw";
            var SMSAccountFrom = "+19073187616";

            var AuthToken = "4a97a25d0fe9309d7f136b346919ca98";
            




            TwilioClient.Init(SMSAccountIdentification, AuthToken);

            MessageResource result = MessageResource.Create(
            new PhoneNumber(message.Destination),
            from: new PhoneNumber(SMSAccountFrom),
            body: message.Body
            );

            
            Trace.TraceInformation(result.Status.ToString());
            
            return;
                   
        }

        Task IIdentityMessageService.SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();

        }
    }
}