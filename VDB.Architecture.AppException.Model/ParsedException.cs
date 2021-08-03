using System.Collections.Generic;
using System.Net;

namespace VDB.Architecture.AppException.Model
{
    public record ParsedException(HttpStatusCode StatusCode, string LogID, IEnumerable<string> Messages)
    {
    }
}
