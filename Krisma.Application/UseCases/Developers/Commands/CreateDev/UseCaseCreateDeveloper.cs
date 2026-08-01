using Krisma.Application.Contracts;
using Krisma.Domain.Common;
using Krisma.Domain.Entities;
using Krisma.Domain.Enums;
using Krisma.Domain.ValueObjects;
using MediatR;
using System.Linq.Expressions;


namespace Krisma.Application.UseCases.Developers.Commands.CreateDev;

public class UseCaseCreateDeveloper(IRepositoryDevelopers developerRepository) : IRequestHandler<CreateDeveloperCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateDeveloperCommand request, CancellationToken cancellationToken)
    {
        var Exists = await developerRepository.ExistsByGitHubLogin(request.GitHubLogin);

        if (Exists)
        {
            return Result.Failure<Guid>(new Error("developer.github.exists", "Developer with the same GitHub login already exists.", ErrorType.Conflict));
        }

        var developerResult = Developer.Create(
            name: request.Name,
            lastName: request.LastName,
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
