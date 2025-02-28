using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.DataAccessLayer.EntityModel.SQL.Model;

public class OpenEmailLog
{
    [Key]
    public int Id { get; set; }
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
    public string? MachineAddress { get; set; }
    public string? UserAgent { get; set; }
    [ForeignKey(nameof(AppsLogs))]
    public long? logId { get; set; }
    public AppsLog? AppsLogs { get; set; }
}
