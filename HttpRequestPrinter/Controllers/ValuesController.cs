using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.HPack;

namespace HttpRequestPrinter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> Post()
        {
            Console.WriteLine("--- Request");
            Console.WriteLine(Request);

            var sb = new StringBuilder();

            sb.AppendLine("--- HEADERS ---");
            var requestStrings = Request.Headers.Select(h => String.Join(": ", h.Key, h.Value));
            foreach (var r in requestStrings)
            {
                sb.AppendLine(r);
            }

            var contentType = Request.Headers.SingleOrDefault(h => h.Key.Equals("Content-Type"));
            sb.AppendLine($"--- CONTENT TYPE ---");
            sb.AppendLine($"Content-Type: {contentType.Value}");

            sb.AppendLine("--- HAS FORM CONTENT TYPE ---");
            sb.AppendLine($"{Request.HasFormContentType}");

            try
            {
                sb.AppendLine("--- FORM ---");
                var form = Request.Form;
                sb.AppendLine($"Request.Form is present.");
                sb.AppendLine($"Request.Form.Count: {form.Count}");
            }
            catch(Exception e)
            {
                sb.AppendLine($"Accessing Request.Form threw {e.GetType()}. Message: {e.Message}");
            }

            try
            {
                sb.AppendLine("--- FORM FIELDS ---");
                sb.AppendLine($"Request.Form.Keys.Count: {Request.Form.Keys.Count}");
                var formFields = Request.Form.ToDictionary(f => f.Key).Values.Select(v => String.Join(": ", v.Key, v.Value));
                foreach (var f in formFields)
                {
                    sb.AppendLine(f);
                }

            }
            catch (Exception e)
            {
                sb.AppendLine($"Accessing Request.Form.ToDictionary(..) threw {e.GetType()}. Message: {e.Message}");
            }

            try
            {
                sb.AppendLine("--- FORM FILES ---");
                var formFiles = Request.Form.Files;
                sb.AppendLine($"Request.Form.Files count: {formFiles.Count}");

                sb.AppendLine($"counter file.Name file.FileName file.Length file.ContentType file.Headers.Count");
                int counter = 1;
                foreach (var file in formFiles)
                {
                    sb.AppendLine($"File #{counter}: {file.Name} {file.FileName} {file.Length} {file.ContentType} {file.Headers.Count}");
                    var fileHeaders = file.Headers.ToDictionary(h => h.Key).Values.Select(v => String.Join(": ", v.Key, v.Value));
                    foreach (var fh in fileHeaders)
                    {
                        sb.AppendLine("         " + fh);
                    }
                    counter++;
                }
                sb.AppendLine($"Sum of length of all form files in bytes: {formFiles.Sum(ff => ff.Length)}");
            }
            catch (Exception e)
            {
                sb.AppendLine($"Accessing Request.Form.Files threw {e.GetType()}. Message: {e.Message}");
            }

            sb.AppendLine("--- BODY ---");
            try
            {
                //var body = Request.Body;
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = reader.ReadToEnd();
                    sb.AppendLine($"Body is {body.Length} bytes long.");
                    sb.AppendLine($"Body content: {body}");

                    // Do something
                }
                //sb.AppendLine($"Body is {Request.Body.Length} bytes long.");
            }
            catch(Exception e)
            {
                sb.AppendLine($"Accessing Request.Body.Length threw {e.GetType()}. Message: {e.Message}");
            }

            return sb.ToString();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
