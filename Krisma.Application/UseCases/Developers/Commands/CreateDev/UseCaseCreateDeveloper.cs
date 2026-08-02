using Krisma.Application.Contracts;
using Krisma.Domain.Common;
using Krisma.Domain.Entities;
using Krisma.Domain.Enums;
using Krisma.Domain.ValueObjects;
using MediatR;
using System.Linq.Expressions;


namespace Krisma.Application.UseCases.Developers.Commands.CreateDev;

public class UseCaseCreateDeveloper(IDevelopersRepository developerRepository) : IRequestHandler<CreateDeveloperCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateDeveloperCommand request, CancellationToken cancellationToken)
    {
        var exists = await developerRepository.ExistsByGitHubUserId(request.OrganizationId, request.GitHubUserId);

        if (exists)
        {
            return Result.Failure<Guid>(new Error("developer.github.exist", "Developer with the same GitHub user already exists in this organization.", ErrorType.Conflict));
        }

        var developerResult = Developer.Create(
            organizationId: request.OrganizationId,
            name: request.Name,
            lastName: request.LastName,
            gitHubUserId: request.GitHubUserId,
            gitHubNodeId: request.GitHubNodeId,
            gitHubLogin: request.GitHubLogin,
            email: request.Email,
            seniority: (Seniority)request.Seniority,
            hireDate: request.HireDate,
            position: (Position)request.Position,
            department: (Department)request.Department
        );

        if (developerResult.IsFailure)
        {
            return Result.Failure<Guid>(developerResult.Error);
        }

        await developerRepository.Add(developerResult.Value);

        return Result.Success(developerResult.Value.Id);
    }
}
