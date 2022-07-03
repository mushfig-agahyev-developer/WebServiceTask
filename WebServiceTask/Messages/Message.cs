using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Messages
{
    public class Message
    {
        public Message() => _messages = new List<string>();
        List<string> _messages { get; set; }
    }
}
