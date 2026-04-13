using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ofix.Models
{
    public interface IModelAppService :
        ICrudAppService<
            ModelDto,
            Guid,
            ModelListInput,
            CreateUpdateModelDto>
    {
    }
}