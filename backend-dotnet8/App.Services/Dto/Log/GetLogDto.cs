using System.ComponentModel.DataAnnotations;

namespace App.Services.Dto.Log;

public class GetLogDto
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? UserName { get; set; }
    public string? Level { get; set; }
    public string? Message { get; set; }
    public string? Exception { get; set; }
    public string? Properties { get; set; }
    public string? LogEvent { get; set; }
}
public class LogCountDto
{
    public string? Level { get; set; }
    public int Total { get; set; }
}
public class GetLogsDetail
{
    public IEnumerable<GetLogDto>? List { get; set; }
    public IEnumerable<LogCountDto>? Total { get; set; }


}
public class OpenApiReqLogDto
{
    public string? UserName { get; set; }
    public string? ReqData { get; set; }  // Stored as VARBINARY(MAX) in SQL Server
    public string? ResData { get; set; }  // Stored as VARBINARY(MAX) in SQL Server
    public string? CallType { get; set; }
    public int? StatusCode { get; set; }
    public string? ServiceUtilizer { get; set; }
    public DateTime ReqTime { get; set; } = DateTime.Now;
    public DateTime? ResTime { get; set; }  // Nullable for cases where response time is not yet set
    public long? ExecutionTime { get; set; }
    public long? TransTraceId { get; set; }

}
public class OpenEmailLogDto
{
    public string? AttachmentsPath { get; set; }
    public bool Attempts { get; set; } = true;
    public DateTime? NotificationSendTime { get; set; }
    public DateTime? NotificationRecordDate { get; set; }
    public string? NotificationSendCc { get; set; }
    public string? NotificationSendBC { get; set; }
    [Required]
    public string? NotificationSendTo { get; set; }
    public string? NotificationSubject { get; set; }
    [Required]
    public string? NotificationText { get; set; }
    public string? SendingStatus { get; set; } = "S";
    public string? NotificationTransType { get; set; }
    public string? Reason { get; set; }
    public string? SendingCount { get; set; }
    public string? UserName { get; set; }
}
