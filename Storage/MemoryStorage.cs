using Infonetica_task.Models;

namespace Infonetica_task.Storage;

public class MemoryStorage : IStorage
{
    private readonly Dictionary<string, WorkflowDefinition> _definitions = new();
    private readonly Dictionary<string, WorkflowInstance> _instances = new();

    public void SaveDefinition(WorkflowDefinition definition)
    {
        _definitions[definition.Id] = definition;
    }

    public WorkflowDefinition? GetDefinition(string id)
    {
        _definitions.TryGetValue(id, out var definition);
        return definition;
    }

    public List<WorkflowDefinition> GetAllDefinitions()
    {
        return _definitions.Values.ToList();
    }

    public void SaveInstance(WorkflowInstance instance)
    {
        _instances[instance.Id] = instance;
    }

    public WorkflowInstance? GetInstance(string id)
    {
        _instances.TryGetValue(id, out var instance);
        return instance;
    }
}