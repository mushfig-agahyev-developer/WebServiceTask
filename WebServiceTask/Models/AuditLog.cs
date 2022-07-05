using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask.Models
{
    public class AuditLog
    {
        public AuditLog()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public byte Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }
        public string Ip { get; set; }
    }
}
