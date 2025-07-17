using Microsoft.AspNetCore.Mvc;
using Infonetica_task.Models;
using Infonetica_task.Services;

namespace Infonetica_task.Controllers;

[ApiController]
[Route("api/workflow")]
public class WorkflowController : ControllerBase
{
    private readonly IWorkflowService _service;

    public WorkflowController(IWorkflowService service)
    {
        _service = service;
    }

    [HttpPost("definitions")]
    public IActionResult CreateDefinition([FromBody] WorkflowDefinition definition)
    {
        try
        {
            var result = _service.CreateDefinition(definition);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("definitions/{id}")]
    public IActionResult GetDefinition(string id)
    {
        var definition = _service.GetDefinition(id);
        if (definition == null)
            return NotFound(new { error = $"Definition '{id}' not found" });
        
        return Ok(definition);
    }

    [HttpGet("definitions")]
    public IActionResult GetAllDefinitions()
    {
        var definitions = _service.GetAllDefinitions();
        return Ok(definitions);
    }

    [HttpPost("instances")]
    public IActionResult StartInstance([FromBody] StartInstanceRequest request)
    {
        try
        {
            var instance = _service.StartInstance(request.DefinitionId);
            return Ok(instance);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("instances/{instanceId}/actions/{actionId}")]
    public IActionResult ExecuteAction(string instanceId, string actionId)
    {
        try
        {
            var instance = _service.ExecuteAction(instanceId, actionId);
            return Ok(instance);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("instances/{id}")]
    public IActionResult GetInstance(string id)
    {
        var instance = _service.GetInstance(id);
        if (instance == null)
            return NotFound(new { error = $"Instance '{id}' not found" });
            
        return Ok(instance);
    }
}

public class StartInstanceRequest
{
    public string DefinitionId { get; set; } = "";
}