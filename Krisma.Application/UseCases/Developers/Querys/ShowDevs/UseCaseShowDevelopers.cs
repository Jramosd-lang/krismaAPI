using Krisma.Application.Contracts;
using Krisma.Application.Mappers;
using Krisma.Application.Mappers;
using Krisma.Application.UseCases.Developers.DTOs;
using Krisma.Domain.Common;
using MediatR;

namespace Krisma.Application.UseCases.Developers.Querys.ShowDevs;

public record GetDevelopersQuery : IRequest<Result<List<DeveloperResponseDto>>>;

public class UseCaseShowDevelopers(IRepositoryDevelopers developerRepository, IDeveloperMapper developerMapper) : IRequestHandler<GetDevelopersQuery, Result<List<DeveloperResponseDto>>>
{
    public async Task<Result<List<DeveloperResponseDto>>> Handle(GetDevelopersQuery request, CancellationToken cancellationToken)
    {
        var developers = await developerRepository.GetAllDevelopers();

        if (developers == null || developers.Count == 0)
        {
            return Result.Failure<List<DeveloperResponseDto>>(new Error("developers.list.empty", "No developers found.", ErrorType.NotFound));
        }

        var developersDto = developerMapper.ToDtoList(developers);

        return Result.Success(developersDto);

        
    }
}