using Krisma.Application.UseCases.Organizations.Commands.CreateOrganization;
using Krisma.Application.UseCases.Organizations.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Krisma.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationDto model)
    {

        CreateOrganizationCommand command = new(
            model.GitHubOrganizationId,
            model.GitHubNodeId,
            model.Name,
            model.TimezoneId
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(Create), new { id = result.Value }, new { id = result.Value });
    }
}