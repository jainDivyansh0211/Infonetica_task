using Infonetica_task.Models;

namespace Infonetica_task.Storage;

public interface IStorage
{
    void SaveDefinition(WorkflowDefinition definition);
    WorkflowDefinition? GetDefinition(string id);
    List<WorkflowDefinition> GetAllDefinitions();
    
    void SaveInstance(WorkflowInstance instance);
    WorkflowInstance? GetInstance(string id);
}