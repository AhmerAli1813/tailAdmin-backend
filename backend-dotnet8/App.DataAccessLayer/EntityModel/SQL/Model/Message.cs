namespace App.DataAccessLayer.EntityModel.SQL.Model;

public class Message : BaseEntity<long>
{
    public string SenderUserName { get; set; }
    public string ReceiverUserName { get; set; }
    public string Text { get; set; }
}
