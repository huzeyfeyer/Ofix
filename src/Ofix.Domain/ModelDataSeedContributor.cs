using System;
using System.Threading.Tasks;
using Ofix.Models;
using Ofix.SubModels;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Ofix
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

            var models = await _modelRepository.GetListAsync();

            foreach (var model in models)
            {
                if (model.Name == "3 Series")
                {
                    await _subModelRepository.InsertAsync(new SubModel
                    {
                        ModelId = model.Id,
                        Name = "320i",
                        OrderNo = 0,
                        ListingStatus = ListingStatus.Active,
                        Slug = "320i"
                    }, autoSave: true);
                }

                if (model.Name == "C-Class")
                {
                    await _subModelRepository.InsertAsync(new SubModel
                    {
                        ModelId = model.Id,
                        Name = "C200",
                        OrderNo = 1,
                        ListingStatus = ListingStatus.Active,
                        Slug = "c200"
                    }, autoSave: true);
                }

                if (model.Name == "A4")
                {
                    await _subModelRepository.InsertAsync(new SubModel
                    {
                        ModelId = model.Id,
                        Name = "2.0 TDI",
                        OrderNo = 2,
                        ListingStatus = ListingStatus.Draft,
                        Slug = "2-0-tdi"
                    }, autoSave: true);
                }
            }
        }
    }
}