using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class PDFController : ApiController
    {

        [HttpGet]
        [Route("api/PDFController/GetPDF/{Pkey}")]
        public HttpResponseMessage GetPDF( [FromUri] int Pkey,[FromUri] string token)
        {
            /*Download PDF*/
            
            var context = new CollegeBuddyEntities();
            var obj = TokenManager.Identifytoken(token);
            if (obj == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
            var fileobj = context.PDFTables.SingleOrDefault(x => x.PKey == Pkey);
            string fileName = fileobj.Storage;
            string path = @"C:\Users\HP\Desktop\CollegeBuddyFiles\" + fileName;
            string file = string.Format(fileName);

            return DownloadFile(path, file);/*Calling DownloadFile Method*/
        }

        [HttpGet]
        [Route("api/PDFController/ViewPDF/{Pkey}")]
        public HttpResponseMessage ViewPDF([FromUri] int Pkey,[FromUri] string token)
        {
            /*View PDF */
            var context = new CollegeBuddyEntities();
            var obj = TokenManager.Identifytoken(token);
            if (obj == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
            var fileobj = context.PDFTables.SingleOrDefault(x => x.PKey == Pkey);
            string fileName = fileobj.Storage;
            string path = @"C:\Users\HP\Desktop\CollegeBuddyFiles\" + fileName;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream fileStream = File.OpenRead(path);/*Setting up filestream from stream class*/
            response.Content = new StreamContent(fileStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return response;
        }
        private HttpResponseMessage DownloadFile(string downloadFilePath, string fileName)
        {
            try
            {
              
                if (!System.IO.File.Exists(downloadFilePath))/*Checking if file exists or not*/
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"not found");
                }

                MemoryStream responseStream = new MemoryStream();/*Setting up MemoryStream from stream class*/
                Stream fileStream = System.IO.File.Open(downloadFilePath, FileMode.Open);

                fileStream.CopyTo(responseStream);
                fileStream.Close();
                responseStream.Position = 0;

                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StreamContent(responseStream);
                string contentDisposition = string.Concat("attachment; filename=", fileName);
                response.Content.Headers.ContentDisposition =
                      ContentDispositionHeaderValue.Parse(contentDisposition);
                return response;
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }


        }
        [HttpPost]
        [Route("api/PDFController/AddToLibrary/{PKey}")]
        public  HttpResponseMessage AddToLibrary([FromUri] string token,[FromUri] int PKey)
        {
            try
            {
                /*Functionality for adding PDFs to Library for a particular user*/
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);/*if token not identified with user,Error presented*/
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var message = DashboardDetails.LibraryPDF(PKey,obj);
                return Request.CreateResponse(HttpStatusCode.OK, message);

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/PDFController/Library")]
        public HttpResponseMessage Library ([FromUri] string token)
        {
            try
            {/*Functionality for fetching Library for a particular user*/
                
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var query = from x in context.LibraryTables join y in context.PDFTables on x.Pkey equals y.PKey where x.ID == obj.ID select new { y.NameOfPDF, y.PKey };/*Join query between LibraryTable and PDFTable*/
                return Request.CreateResponse(HttpStatusCode.OK, query);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/PDFController/SubjectPDF")]
        public HttpResponseMessage SubjectPDF ([FromUri] string token, [FromUri] int key)
        {
            try
            {
                /*Fetches PDFs of a particular subject selected from subject list*/
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
               var listPdf = context.PDFTables.Where(x => x.Key == key).Select(p => new { p.NameOfPDF, p.PKey }).ToList();/*Selecting PDF Corresponding to subject key*/
                return Request.CreateResponse(HttpStatusCode.OK, listPdf);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage PdfDetails([FromUri] string token,[FromUri] int Pkey)
        {
            try
            {
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var details = context.PDFDetails.Where(x => x.PKey == Pkey).Select(p => new { p.Review, p.Downloads, p.Rating }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, details);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        [HttpPost]
        [Route("api/PDFController/DeleteLibrary/{Pkey}")]
        public HttpResponseMessage DeleteLibrary([FromUri] string token ,[FromUri] int Pkey)
        {
            try {
                /*Delete existing pdf from library*/
                var context = new CollegeBuddyEntities();
                var obj = TokenManager.Identifytoken(token);
                if (obj == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token");
                var PDF = context.LibraryTables.SingleOrDefault(x => x.Pkey == Pkey && x.ID==obj.ID);
                context.LibraryTables.Remove(PDF);
                //var PDF = context.LibraryTables.SqlQuery("Delete from LibraryTable where Pkey="+Pkey+"and ID="+obj.ID);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "PDF Deleted");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }
    }
}
