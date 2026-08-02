using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Application.UseCases.Organizations.DTOs;

public sealed record CreateOrganizationDto(
    long GitHubOrganizationId,
    string GitHubNodeId,
    string Name,
    string TimezoneId
);
