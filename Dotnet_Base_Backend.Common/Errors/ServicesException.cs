using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Base_Backend.Common.Errors
{
    public class ServicesException : Exception
    {
        public ErrorCode ErrorCode { get; }
        private static readonly ResourceManager _resourceManager = ErrorMessages.ResourceManager;

        public ServicesException(ErrorCode errorCode) : base(GetErrorMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        public ServicesException(ErrorCode errorCode, Exception innerException) : base(GetErrorMessage(errorCode), innerException)
        {
            ErrorCode = errorCode;
        }

        private static string GetErrorMessage(ErrorCode errorCode)
        {
            return _resourceManager.GetString(((int)errorCode).ToString(), CultureInfo.CurrentCulture) ?? "Unknown error.";
        }
        public string GetErrorMessage(string language)
        {
            if (string.IsNullOrEmpty(language))
                return GetErrorMessage(ErrorCode);
            return _resourceManager.GetString(((int)ErrorCode).ToString(), new CultureInfo(language)) ?? "Unknown error.";
        }
        public HttpStatusCode HttpStatusCode()
        {
            return (HttpStatusCode)(new ErrorsStatusCodes()[ErrorCode]);
        }
    }
}
