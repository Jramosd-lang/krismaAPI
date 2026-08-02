using Krisma.Application.UseCases.Developers.Commands.ActivateDev;
using Krisma.Application.UseCases.Developers.Commands.CreateDev;
using Krisma.Application.UseCases.Developers.Commands.DeactivateDev;
using Krisma.Application.UseCases.Developers.Queries.GetDeveloperById;
using Krisma.Application.UseCases.Developers.Queries.GetDevelopersByOrganization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Krisma.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevelopersController : ApiControllerBase
{
    private readonly IMediator Mediator;
    public DevelopersController(IMediator Mediator)
    {
        this.Mediator = Mediator;
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetDeveloperByIdQuery(id));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpGet("organization/{id:guid}")]
    public async Task<IActionResult> GetDevelopersByOrganization(Guid id)
    {
        var result = await Mediator.Send(new GetDevelopersByOrganizationQuery(id));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDeveloperDto model)
    {
        CreateDeveloperCommand command = new(
            model.OrganizationId,
            model.Name,
            model.LastName,
            model.GitHubUserId,
            model.GitHubNodeId,
            model.GitHubLogin,
            model.Email,
            model.Seniority,
            model.HireDate,
            model.Position,
            model.Department
        );

        var result = await Mediator.Send(command);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var result = await Mediator.Send(new DeactivateDeveloperCommand(id));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(NoContent());
    }

    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var result = await Mediator.Send(new ActivateDeveloperCommand(id));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(NoContent());
    }
}