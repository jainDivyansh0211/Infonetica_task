namespace Infonetica_task.Models;

public class WorkflowDefinition
{
    public string Id { get; set; } = "";
    public List<State> States { get; set; } = new();
    public List<Action> Actions { get; set; } = new();
    
    public State GetInitialState()
    {
        return States.First(s => s.IsInitial);
    }
    
    public Action GetAction(string actionId)
    {
        return Actions.First(a => a.Id == actionId);
    }
}