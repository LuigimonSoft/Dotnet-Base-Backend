using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Base_Backend.Common.Errors
{
    public class Error
    {
        public int Status { set; get; }
        public ErrorCode Code { set; get; }
        public string Title { set; get; }
        public string Detail { set; get; }
        public string Type { set; get; }
        public string Instance { set; get; }
        public Dictionary<string, string> AdditionalProperties { set; get; }

        public Error() { }
        public Error(ErrorCode errorcode, string Message ,TypeErrors typeError,  Exception error, string Method = "", string url="")
        {             
            Status = 0;
            Code = errorcode;
            Title = Message;
            Detail = error.Message;
            Type = url;
            Instance = Method;
            AdditionalProperties = new Dictionary<string, string>();
#if DEBUG
            AdditionalProperties.Add("Trace", typeError.ToString());
#endif
        }

    }
}
