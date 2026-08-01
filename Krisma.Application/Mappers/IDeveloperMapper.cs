using Krisma.Application.UseCases.Developers.DTOs;
using Krisma.Domain.Entities;
using System.Collections.Generic;

namespace Krisma.Application.Mappers;

public interface IDeveloperMapper
{
    DeveloperResponseDto ToDto(Developer entity);
    Developer ToEntity(DeveloperResponseDto dto);
    List<DeveloperResponseDto> ToDtoList(IEnumerable<Developer> entities);
    List<Developer> ToEntityList(IEnumerable<DeveloperResponseDto> dtos);
}