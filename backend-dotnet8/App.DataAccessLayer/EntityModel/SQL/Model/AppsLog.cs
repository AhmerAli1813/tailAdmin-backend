namespace App.DataAccessLayer.EntityModel.SQL.Model;

public class AppsLog : BaseEntity<long>
{
    public string? UserName { get; set; }
    public string? Level { get; set; }
    public string? Message { get; set; }
    public string? Exception { get; set; }
    public string? Properties { get; set; }
    public string? LogEvent { get; set; }
}
