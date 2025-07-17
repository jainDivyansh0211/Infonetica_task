using Infonetica_task.Models;
using Infonetica_task.Storage;

namespace Infonetica_task.Services;

public class WorkflowService : IWorkflowService
{
    private readonly IStorage _storage;

    public WorkflowService(IStorage storage)
    {
        _storage = storage;
    }

    public WorkflowDefinition CreateDefinition(WorkflowDefinition definition)
    {
        ValidateDefinition(definition);
        _storage.SaveDefinition(definition);
        return definition;
    }

    public WorkflowDefinition? GetDefinition(string id)
    {
        return _storage.GetDefinition(id);
    }

    public List<WorkflowDefinition> GetAllDefinitions()
    {
        return _storage.GetAllDefinitions();
    }

    public WorkflowInstance StartInstance(string definitionId)
    {
        var definition = GetDefinitionOrThrow(definitionId);
        var initialState = definition.GetInitialState();
        
        var instance = new WorkflowInstance
        {
            DefinitionId = definitionId,
            CurrentState = initialState.Id
        };

        _storage.SaveInstance(instance);
        return instance;
    }

    public WorkflowInstance ExecuteAction(string instanceId, string actionId)
    {
        var instance = GetInstanceOrThrow(instanceId);
        var definition = GetDefinitionOrThrow(instance.DefinitionId);
        var action = definition.GetAction(actionId);

        ValidateActionExecution(instance, action, definition);
        
        MoveInstanceToNewState(instance, action);
        _storage.SaveInstance(instance);
        
        return instance;
    }

    public WorkflowInstance? GetInstance(string id)
    {
        return _storage.GetInstance(id);
    }

    private WorkflowDefinition GetDefinitionOrThrow(string id)
    {
        var definition = _storage.GetDefinition(id);
        if (definition == null)
            throw new ArgumentException($"Definition '{id}' not found");
        return definition;
    }

    private WorkflowInstance GetInstanceOrThrow(string id)
    {
        var instance = _storage.GetInstance(id);
        if (instance == null)
            throw new ArgumentException($"Instance '{id}' not found");
        return instance;
    }

    private static void MoveInstanceToNewState(WorkflowInstance instance, Models.Action action)
    {
        var historyEntry = new HistoryEntry
        {
            ActionId = action.Id,
            FromState = instance.CurrentState,
            ToState = action.ToState
        };

        instance.CurrentState = action.ToState;
        instance.History.Add(historyEntry);
    }

    // Comprehensive validation for workflow definitions
    private static void ValidateDefinition(WorkflowDefinition definition)
    {
        ThrowIfEmptyId(definition.Id, "Definition ID");
        ThrowIfNoStates(definition.States);
        ThrowIfNotExactlyOneInitialState(definition.States);
        ThrowIfDuplicateStateIds(definition.States);
        ThrowIfDuplicateActionIds(definition.Actions);
        ThrowIfActionsReferenceInvalidStates(definition);
    }

    // Runtime validation before executing actions
    private static void ValidateActionExecution(WorkflowInstance instance, Models.Action action, WorkflowDefinition definition)
    {
        ThrowIfActionDisabled(action);
        ThrowIfActionNotAllowedFromCurrentState(instance, action);
        ThrowIfInstanceInFinalState(instance, definition);
    }

    // Validation helper methods with clear error messages
    private static void ThrowIfEmptyId(string id, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException($"{fieldName} cannot be empty");
    }

    private static void ThrowIfNoStates(List<State> states)
    {
        if (!states.Any())
            throw new ArgumentException("Definition must have at least one state");
    }

    private static void ThrowIfNotExactlyOneInitialState(List<State> states)
    {
        var initialStates = states.Where(s => s.IsInitial).ToList();
        if (initialStates.Count == 0)
            throw new ArgumentException("Definition must have exactly one initial state");
        if (initialStates.Count > 1)
            throw new ArgumentException("Definition can only have one initial state");
    }

    private static void ThrowIfDuplicateStateIds(List<State> states)
    {
        var stateIds = states.Select(s => s.Id).ToList();
        if (stateIds.Count != stateIds.Distinct().Count())
            throw new ArgumentException("Duplicate state IDs found");
    }

    private static void ThrowIfDuplicateActionIds(List<Models.Action> actions)
    {
        var actionIds = actions.Select(a => a.Id).ToList();
        if (actionIds.Count != actionIds.Distinct().Count())
            throw new ArgumentException("Duplicate action IDs found");
    }

    // Ensure all action transitions reference valid states
    private static void ThrowIfActionsReferenceInvalidStates(WorkflowDefinition definition)
    {
        var stateIds = definition.States.Select(s => s.Id).ToHashSet();
        
        foreach (var action in definition.Actions)
        {
            if (!stateIds.Contains(action.ToState))
                throw new ArgumentException($"Action '{action.Id}' references invalid ToState '{action.ToState}'");

            foreach (var fromState in action.FromStates)
            {
                if (!stateIds.Contains(fromState))
                    throw new ArgumentException($"Action '{action.Id}' references invalid FromState '{fromState}'");
            }
        }
    }

    private static void ThrowIfActionDisabled(Models.Action action)
    {
        if (!action.Enabled)
            throw new InvalidOperationException($"Action '{action.Id}' is disabled");
    }

    private static void ThrowIfActionNotAllowedFromCurrentState(WorkflowInstance instance, Models.Action action)
    {
        if (!action.FromStates.Contains(instance.CurrentState))
            throw new InvalidOperationException($"Action '{action.Id}' cannot be executed from state '{instance.CurrentState}'");
    }

    private static void ThrowIfInstanceInFinalState(WorkflowInstance instance, WorkflowDefinition definition)
    {
        var currentState = definition.States.First(s => s.Id == instance.CurrentState);
        if (currentState.IsFinal)
            throw new InvalidOperationException("Cannot execute actions on instances in final states");
    }
}