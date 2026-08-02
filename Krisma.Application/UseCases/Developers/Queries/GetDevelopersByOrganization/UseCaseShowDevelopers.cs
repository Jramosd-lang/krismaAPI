using Krisma.Application.Contracts;
using Krisma.Application.Mappers;
using Krisma.Application.UseCases.Developers.DTOs;
using Krisma.Domain.Common;
using MediatR;

namespace Krisma.Application.UseCases.Developers.Queries.GetDevelopersByOrganization;

public record GetDevelopersByOrganizationQuery(Guid OrganizationId) : IRequest<Result<List<DeveloperResponseDto>>>;

public class GetDevelopersByOrganizationQueryHandler(
    IDevelopersRepository developerRepository,
    IDeveloperMapper developerMapper)
    : IRequestHandler<GetDevelopersByOrganizationQuery, Result<List<DeveloperResponseDto>>>
{
    public async Task<Result<List<DeveloperResponseDto>>> Handle(GetDevelopersByOrganizationQuery request, CancellationToken cancellationToken)
    {

        var developers = await developerRepository.GetByOrganizationId(request.OrganizationId);

        if (developers is null || !developers.Any())
        {
            return Result.Success(new List<DeveloperResponseDto>());
        }

        var developerDtos = developerMapper.ToDtoList(developers);

        return Result.Success(developerDtos);
    }
}