using System;
using System.Collections.Generic;
using System.Text;

namespace Krisma.Application.Mappers;

public interface IMapper<TEntity, TDto>
{
    TDto ToDto(TEntity entity);
    TEntity ToEntity(TDto dto);
    List<TDto> ToDtoList(IEnumerable<TEntity> entities);
    List<TEntity> ToEntityList(IEnumerable<TDto> dtos);
}