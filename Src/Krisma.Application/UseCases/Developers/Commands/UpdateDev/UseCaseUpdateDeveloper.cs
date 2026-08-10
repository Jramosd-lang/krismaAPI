using Krisma.Application.Contracts;
using Krisma.Domain.Common;
using MediatR;

namespace Krisma.Application.UseCases.Developers.Commands.UpdateDev;

public class UseCaseUpdateDeveloper(IDevelopersRepository developerRepository) : IRequestHandler<UpdateDeveloperCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateDeveloperCommand request, CancellationToken cancellationToken)
    {

        var developer = await developerRepository.GetById(request.Id);

        if (developer == null)
        {
            return Result.Failure<Guid>(new Error("developer.notfound", "Developer not found.", ErrorType.NotFound));
        }

        developer.Update(
            request.Name,
            request.LastName, 
            request.GitHubUserId, 
            request.GitHubNodeId, 
            request.GitHubLogin, 
            request.Email, 
            request.Seniority, 
            request.HireDate, 
            request.Position, 
            request.Department);
        

        await developerRepository.UpdateDeveloper(developer);

        return Result.Success(developer.Id);
    }
}