using Ofix.Brands;
using Ofix.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;


namespace Ofix
{
    public class BrandDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Brand, Guid> _brandRepository;

        public BrandDataSeederContributor(IRepository<Brand, Guid> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _brandRepository.GetCountAsync() <= 0)
            {
                await _brandRepository.InsertAsync(
                    new Brand
                    {
                        Name = "BMW",
                        OrderNo = 0,
                        Status = ListingStatus.Active,
                        LogoBlobName = null,
                        LogoFileName = null,
                        Slug = "bmw",
                    },
                    autoSave: true
                );

                await _brandRepository.InsertAsync(
                    new Brand
                    {
                        Name = "Mercedes",
                        OrderNo = 1,
                        Status = ListingStatus.Active,
                        LogoBlobName = null,
                        LogoFileName = null,
                        Slug = "mercedes",
                    },
                    autoSave: true
                );

                await _brandRepository.InsertAsync(
                    new Brand
                    {
                        Name = "Audi",
                        OrderNo = 2,
                        Status = ListingStatus.Active,
                        LogoBlobName = null,
                        LogoFileName = null,
                        Slug = "audi",
                    },
                    autoSave: true
                );

                await _brandRepository.InsertAsync(
                    new Brand
                    {
                        Name = "Volkswagen",
                        OrderNo = 3,
                        Status = ListingStatus.Active,
                        LogoBlobName = null,
                        LogoFileName = null,
                        Slug = "volkswagen",
                    },
                    autoSave: true
                );



            }
        }


    }
}
