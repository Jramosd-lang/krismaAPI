using Krisma.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Application.UseCases.Organizations.Commands.CreateOrganization;

public record CreateOrganizationCommand
(
    long GitHubOrganizationId,
    string GitHubNodeId,
    string Name,
    string TimezoneId
) : IRequest<Result<Guid>>;
