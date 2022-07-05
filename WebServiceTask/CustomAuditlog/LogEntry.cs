using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceTask.Enums;
using WebServiceTask.Helpers;
using WebServiceTask.Models;

namespace WebServiceTask.CustomAuditlog
{
    public class LogEntry
    {
        public LogEntry(EntityEntry entry, string username)
        {
            Entry = entry;
            UserName = username;
        }
        public EntityEntry Entry { get; }
        public string UserName { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();

        public AuditLog ToAudit()
        {
            var audit = new AuditLog();
            audit.UserName = UserName;
            audit.Type = (byte)AuditType;
            audit.TableName = TableName;
            audit.DateTime = DateTime.Now;
            audit.PrimaryKey = CustomJsonConverter.Serialize(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : CustomJsonConverter.Serialize(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : CustomJsonConverter.Serialize(NewValues);
            audit.AffectedColumns = ChangedColumns.Count == 0 ? null : CustomJsonConverter.Serialize(ChangedColumns);
            return audit;
        }
    }
}
