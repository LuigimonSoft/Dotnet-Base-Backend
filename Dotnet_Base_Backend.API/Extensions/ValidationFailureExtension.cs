using System.Globalization;
using System.Net;
using System.Resources;
using Dotnet_Base_Backend.Common.Errors;

namespace Dotnet_Base_Backend.API.Extensions
{
    public static class ValidationFailureExtension
    {
        private static readonly ResourceManager _resourceManager = new ResourceManager("Dotnet_Base_Backend.Commons.Errors.ErrorMessages", typeof(Error).Assembly);
        public static HttpStatusCode HttpStatusCode(this FluentValidation.Results.ValidationFailure obj)
        {
            return (HttpStatusCode)(new ErrorsStatusCodes()[((ErrorCode)int.Parse(obj.ErrorCode))]);
        }

        public static string GetErrorMessage(this FluentValidation.Results.ValidationFailure obj, string language)
        {
            int errorcode = int.Parse(obj.ErrorCode);

            return _resourceManager.GetString((errorcode).ToString(), new CultureInfo(language)) ?? "Unknown error.";
        }

    }
}
