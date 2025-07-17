using Infonetica_task.Models;

namespace Infonetica_task.Services;

public interface IWorkflowService
{
    WorkflowDefinition CreateDefinition(WorkflowDefinition definition);
    WorkflowDefinition? GetDefinition(string id);
    List<WorkflowDefinition> GetAllDefinitions();
    
    WorkflowInstance StartInstance(string definitionId);
    WorkflowInstance ExecuteAction(string instanceId, string actionId);
    WorkflowInstance? GetInstance(string id);
}