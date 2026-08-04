using Krisma.Application.Contracts;
using Krisma.Application.Mappers;
using Krisma.Application.UseCases.Developers.DTOs;
using Krisma.Domain.Common;
using MediatR;

namespace Krisma.Application.UseCases.Developers.Queries.GetDeveloperById;

public record GetDeveloperByIdQuery(Guid Id) : IRequest<Result<DeveloperResponseDto>>;

public class GetDeveloperByIdQueryHandler(
    IDevelopersRepository developerRepository,
    IDeveloperMapper developerMapper)
    : IRequestHandler<GetDeveloperByIdQuery, Result<DeveloperResponseDto>>
{
    public async Task<Result<DeveloperResponseDto>> Handle(GetDeveloperByIdQuery request, CancellationToken cancellationToken)
    {
        var developer = await developerRepository.GetById(request.Id);

        if (developer is null)
        {
            return Result.Failure<DeveloperResponseDto>(new Error("developer.exists.notfount", "developer not found", ErrorType.NotFound));
        }

        var developerDto = developerMapper.ToDto(developer);

        return Result.Success(developerDto);
    }
}