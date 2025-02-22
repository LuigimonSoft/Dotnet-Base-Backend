using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Dotnet_Base_Backend.Common.Errors
{
    public class ErrorsStatusCodes: Dictionary<ErrorCode, HttpStatusCode>
    {
        public ErrorsStatusCodes() 
        {
            Add(ErrorCode.REQUIRED, HttpStatusCode.BadRequest);
            Add(ErrorCode.EMPTY, HttpStatusCode.BadRequest);
            Add(ErrorCode.MAX_LENGTH, HttpStatusCode.BadRequest);
            Add(ErrorCode.INVALID_JSON_FORMAT, HttpStatusCode.BadRequest);
            Add(ErrorCode.INVALID, HttpStatusCode.BadRequest);
            Add(ErrorCode.NOT_FOUND, HttpStatusCode.NotFound);
            Add(ErrorCode.UNEXPECTED_JSON_FORMAT, HttpStatusCode.BadRequest);
            Add(ErrorCode.DATABASE_ERROR, HttpStatusCode.InternalServerError);
            Add(ErrorCode.INTERNAL_SERVER_ERROR, HttpStatusCode.InternalServerError);
            Add(ErrorCode.DEPENDENCY_INJECTION_ERROR, HttpStatusCode.InternalServerError);
        }
    }
}
