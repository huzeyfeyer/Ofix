using Ofix.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Ofix.Brands;

namespace Ofix
{
    public class ModelDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Model, Guid> _modelRepository;
        private readonly IRepository<Brand, Guid> _brandRepository;
    

    public ModelDataSeedContributor(IRepository<Model, Guid> modelRepository, IRepository<Brand, Guid> brandRepository)
    {
            _modelRepository = modelRepository;
            _brandRepository = brandRepository;
    }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _modelRepository.GetCountAsync() <= 0)
            {
                var bmwBrand = await _brandRepository.FirstOrDefaultAsync(b => b.Name == "BMW");
                var mercedesBrand = await _brandRepository.FirstOrDefaultAsync(b => b.Name == "Mercedes");
                var audiBrand = await _brandRepository.FirstOrDefaultAsync(b => b.Name == "Audi");
                var volkswagenBrand = await _brandRepository.FirstOrDefaultAsync(b => b.Name == "Volkswagen");
                if (bmwBrand != null)
                {
                    await _modelRepository.InsertAsync(
                        new Model
                        {
                            BrandId = bmwBrand.Id,
                            Name = "3 Series",
                            OrderNo = 0,
                            ListingStatus = Models.ListingStatus.Active,
                            Slug = "3-series",
                        },
                        autoSave: true
                    );
                }
                if (mercedesBrand != null)
                {
                    await _modelRepository.InsertAsync(
                        new Model
                        {
                            BrandId = mercedesBrand.Id,
                            Name = "C-Class",
                            OrderNo = 1,
                            ListingStatus = Models.ListingStatus.Sold,
                            Slug = "c-class",
                        },
                        autoSave: true
                    );
                }
                if (audiBrand != null)
                {
                    await _modelRepository.InsertAsync(
                        new Model
                        {
                            BrandId = audiBrand.Id,
                            Name = "A4",
                            OrderNo = 2,
                            ListingStatus = Models.ListingStatus.Expired,
                            Slug = "a4",
                        },
                        autoSave: true
                    );
                }
                if (volkswagenBrand != null)
                {
                    await _modelRepository.InsertAsync(
                        new Model
                        {
                            BrandId = volkswagenBrand.Id,
                            Name = "Polo",
                            OrderNo = 3,
                            ListingStatus = Models.ListingStatus.Active,
                            Slug = "polo",
                        }, autoSave: true
                        );
                }
            }

        }
    }
}
