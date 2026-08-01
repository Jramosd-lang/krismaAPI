using Krisma.Application.UseCases.Developers.Commands.CreateDev;
using Krisma.Application.UseCases.Developers.DTOs;
using Krisma.Application.UseCases.Developers.Querys.ShowDevs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Krisma.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevelopersController : ControllerBase
{
    private readonly IMediator Mediator;
    public DevelopersController(IMediator Mediator)
    {
        this.Mediator = Mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DeveloperResponseDto>>> Get()
    {
        var result = await Mediator.Send(new GetDevelopersQuery());

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateDeveloperDto model)
    {
        CreateDeveloperCommand command = new(
            model.Name,
            model.LastName,
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
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}