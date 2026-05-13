using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ofix.Models;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Ofix.SubModels
{
    public class SubModelDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<SubModel, Guid> _subModelRepository;
        private readonly IRepository<Model, Guid> _modelRepository;

        public SubModelDataSeedContributor(
            IRepository<SubModel, Guid> subModelRepository,
            IRepository<Model, Guid> modelRepository)
        {
            _subModelRepository = subModelRepository;
            _modelRepository = modelRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _subModelRepository.GetCountAsync() > 0)
            {
                return;
            }

            var bmw3Series = await _modelRepository.FirstOrDefaultAsync(x => x.Name == "3 Series");
            var mercedesCClass = await _modelRepository.FirstOrDefaultAsync(x => x.Name == "C-Class");
            var audiA4 = await _modelRepository.FirstOrDefaultAsync(x => x.Name == "A4");

            if (bmw3Series != null)
            {
                await _subModelRepository.InsertAsync(new SubModel
                {
                    ModelId = bmw3Series.Id,
                    Name = "320i",
                    OrderNo = 0,
                    IsActive = true,
                    Slug = "320i"
                }, autoSave: true);
            }

            if (mercedesCClass != null)
            {
                await _subModelRepository.InsertAsync(new SubModel
                {
                    ModelId = mercedesCClass.Id,
                    Name = "C200",
                    OrderNo = 1,
                    IsActive = true,
                    Slug = "c200"
                }, autoSave: true);
            }

            if (audiA4 != null)
            {
                await _subModelRepository.InsertAsync(new SubModel
                {
                    ModelId = audiA4.Id,
                    Name = "2.0 TDI",
                    OrderNo = 2,
                    IsActive = false,
                    Slug = "2-0-tdi"
                }, autoSave: true);
            }
        }
    }
}
