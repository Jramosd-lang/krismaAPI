using Krisma.Application.Contracts;
using Krisma.Domain.Common;
using MediatR;

namespace Krisma.Application.UseCases.Developers.Commands.DeactivateDev;

public record DeactivateDeveloperCommand(Guid Id) : IRequest<Result>;

public class UseCaseDeactivateDeveloper(IDevelopersRepository developersRepository) : IRequestHandler<DeactivateDeveloperCommand, Result>
{
    public async Task<Result> Handle(DeactivateDeveloperCommand request, CancellationToken cancellationToken)
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