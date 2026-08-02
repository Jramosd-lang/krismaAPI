using Krisma.Application.Contracts;
using Krisma.Application.UseCases.Organizations.Commands.CreateOrganization;
using Krisma.Domain.Common;
using Krisma.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Application.UseCases.Organizations.Commands.CreateOrganization;

public class UseCaseCreateOrganization(IOrganizationsRepository organizationsRepository) : IRequestHandler<CreateOrganizationCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var exists = await organizationsRepository.Exists(request.Name, request.GitHubOrganizationId, request.GitHubNodeId);
        
        if (exists)
        {
            return Result.Failure<Guid>(new Error("Organization.name.exist", "Organization with the same name already exists.", ErrorType.Conflict));
        }

        var organizationResult = Organization.Create(
            request.GitHubOrganizationId, 
            request.GitHubNodeId, 
            request.Name, 
            request.TimezoneId);

        if (organizationResult.IsFailure)
        {
            return Result.Failure<Guid>(organizationResult.Error);
        }

        await organizationsRepository.Add(organizationResult.Value);

        return Result.Success(organizationResult.Value.Id);
    }
}
