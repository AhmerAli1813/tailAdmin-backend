using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model;

public class OpenApiReqLog
{
    [Key]
    public long Id { get; set; }
    public string? MachineAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? ReqData { get; set; }  // Stored as VARBINARY(MAX) in SQL Server
    public string? ResData { get; set; }  // Stored as VARBINARY(MAX) in SQL Server
    public string? CallType { get; set; }
    public string? ServiceUtilizer { get; set; }
    public int? StatusCode { get; set; }

    public DateTime ReqTime { get; set; } = DateTime.Now;
    public DateTime? ResTime { get; set; }  // Nullable for cases where response time is not yet set
    public long? ExecutionTime { get; set; }
    [ForeignKey(nameof(AppsLogs))]
    public long? TransTraceId { get; set; }
    public AppsLog? AppsLogs { get; set; }

}

