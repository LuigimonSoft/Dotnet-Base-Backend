using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Base_Backend.Common.Errors
{
    public enum ErrorCode
    {
        REQUIRED = 1001,
        EMPTY = 1002,
        MAX_LENGTH = 1003,
        NOT_FOUND = 1004,
        INVALID = 1005,
        INVALID_JSON_FORMAT = 1013,
        UNEXPECTED_JSON_FORMAT = 1014,
        DATABASE_ERROR = 4001,
        INTERNAL_SERVER_ERROR = 5001,
        DEPENDENCY_INJECTION_ERROR = 5002
    }
}
