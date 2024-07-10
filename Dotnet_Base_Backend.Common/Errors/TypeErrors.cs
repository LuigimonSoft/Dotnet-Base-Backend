using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Base_Backend.Common.Errors
{
    public enum TypeErrors
    {
        REPOSITORY_ERROR ,
        SERVICE_ERROR,
        CONTROLLER_ERROR,
        VALIDATION_ERROR,
        CUSTOM_ERROR,
        UNKNOWN_ERROR,
        INFRASTRUCTURE_ERROR
    }
}
