using System.Collections.Generic;
using System.Net;

namespace VDB.Architecture.AppException.Model
{
    public record ParsedException
    {
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// If this is a logged exception logs id will be present here.
        /// </summary>
        public string LogID { get; set; }
        /// <summary>
        /// User friendly error messages.
        /// </summary>
        public IEnumerable<string> Messages { get; set; }

        public ParsedException(HttpStatusCode statusCode, string logID, IEnumerable<string> messages)
        {
            StatusCode = statusCode;
            LogID = logID;
            Messages = messages;
        }
    }
}
