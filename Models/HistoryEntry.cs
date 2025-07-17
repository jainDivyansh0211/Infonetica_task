namespace Infonetica_task.Models;

public class HistoryEntry
{
    public string ActionId { get; set; } = "";
    public string FromState { get; set; } = "";
    public string ToState { get; set; } = "";
    public DateTime When { get; set; } = DateTime.Now;
}