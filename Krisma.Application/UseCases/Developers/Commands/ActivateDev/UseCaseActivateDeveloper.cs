using Krisma.Application.Contracts;
using Krisma.Domain.Common;
using MediatR;

namespace Krisma.Application.UseCases.Developers.Commands.ActivateDev;

public record ActivateDeveloperCommand(Guid Id) : IRequest<Result>;

public class UseCaseActivateDeveloper(IDevelopersRepository developersRepository) : IRequestHandler<ActivateDeveloperCommand, Result>
{
    public async Task<Result> Handle(ActivateDeveloperCommand request, CancellationToken cancellationToken)
    {
        var developer = await developersRepository.GetById(request.Id);

        if (developer is null)
        {
            return Result.Failure(new Error("developer.not.found", "Developer not found.", ErrorType.NotFound));
        }

        await developersRepository.DeactivateDeveloper(request.Id);

        return Result.Success();
    }
}