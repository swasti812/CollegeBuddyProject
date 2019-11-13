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


namespace WebApplication3.Models
{
    public class MemberDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string College { get; set; }
        public string Branch { get; set; }
        public string Year { get; set; }
        public string MobileNo { get; set; }
        public string Image { get; set; }
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
            var SMSAccountIdentification = "AC206f1df0fcf87ece093f014b4ce1cb64";
            //var SMSAccountPassword = "Swastitiwari123";
            var SMSAccountFrom = "+14025528331";
            var AuthToken = "9af6c0e4807fd60dced4b14ec977b661";
            //var accountSid = ConfigurationManager.AppSettings["SMSAccountIdentification"];
            //var authToken = ConfigurationManager.AppSettings["SMSAccountPassword"];
            //var fromNumber = ConfigurationManager.AppSettings["SMSAccountFrom"];





            TwilioClient.Init(SMSAccountIdentification, AuthToken);

            MessageResource result = MessageResource.Create(
            new PhoneNumber(message.Destination),
            from: new PhoneNumber(SMSAccountFrom),
            body: message.Body
            );

            //Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            Trace.TraceInformation(result.Status.ToString());
            //Twilio doesn't currently have an async API, so return success.
            return;// Task.FromResult(0);
                   //Twilio End
        }

        Task IIdentityMessageService.SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();

        }
    }
}