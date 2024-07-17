using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Base_Backend.Models
{
    public class MessagesModel: List<string>
    {
        public MessagesModel()
        {
        }

        public MessagesModel(List<string> list)
        {
            this.toMessagesModel(list);
        }
        public void toMessagesModel(List<string> list)
        {
            foreach (var item in list)
            {
                this.Add(item);
            }
            
        }
    }
}
