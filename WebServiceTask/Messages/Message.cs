using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Messages
{
    public class Message
    {
        public Message() => Messages = new List<string>();
        public List<string> Messages { get; set; }
    }
}
