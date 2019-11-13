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
        public HttpResponseMessage GetPDF( [FromUri] int Pkey)
        {
            var context = new CollegeBuddyEntities();

            var fileobj = context.PDFTables.SingleOrDefault(x => x.PKey == Pkey);
            string fileName = fileobj.Storage;
            string path= @"C:\Users\HP\Desktop\CollegeBuddyFiles\"+fileName;
            //string filePath = string.Format(@"{0}\{1}\{2}\{3}\{4}.{5}", "C:", "Users", "HP",
            //"Desktop", fileName, ".pdf");
            string file = string.Format(fileName);

            return DownloadFile(path, file);
        }

        [HttpGet]
        [Route("api/PDFController/ViewPDF/{Pkey}")]
        public HttpResponseMessage ViewPDF([FromUri] int Pkey)
        {
            var context = new CollegeBuddyEntities();
            var fileobj = context.PDFTables.SingleOrDefault(x => x.PKey == Pkey);
            string fileName = fileobj.Storage;
            string path = @"C:\Users\HP\Desktop\CollegeBuddyFiles\" + fileName;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream fileStream = File.OpenRead(path);
            response.Content = new StreamContent(fileStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return response;
        }
        private HttpResponseMessage DownloadFile(string downloadFilePath, string fileName)
        {
            try
            {
                //Check if the file exists. If the file doesn't exist, throw a file not found exception
                if (!System.IO.File.Exists(downloadFilePath))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"not found");
                }

                //Copy the source file stream to MemoryStream and close the file stream
                MemoryStream responseStream = new MemoryStream();
                Stream fileStream = System.IO.File.Open(downloadFilePath, FileMode.Open);

                fileStream.CopyTo(responseStream);
                fileStream.Close();
                responseStream.Position = 0;

                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;

                //Write the memory stream to HttpResponseMessage content
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
    }
}
