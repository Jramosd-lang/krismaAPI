using Krisma.Application.UseCases.Developers.DTOs;
using Krisma.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Krisma.Application.Mappers;

public class DeveloperMapper : IDeveloperMapper
{
    public DeveloperResponseDto ToDto(Developer entity)
    {
        return new DeveloperResponseDto(
            entity.Id,
            entity.Name,
            entity.LastName,
            entity.GitHubLogin,
            entity.Email.Value,
            (int)entity.Seniority,
            entity.HireDate,
            (int)entity.Position,
            entity.IsActive,
            (int)entity.Department
        );
    }

    public Developer ToEntity(DeveloperResponseDto dto)
    {
        throw new System.NotImplementedException();
    }

    public List<DeveloperResponseDto> ToDtoList(IEnumerable<Developer> entities)
    {
        return entities.Select(ToDto).ToList();
    }

    public List<Developer> ToEntityList(IEnumerable<DeveloperResponseDto> dtos)
    {
        throw new System.NotImplementedException();
    }
}