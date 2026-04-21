using Microsoft.AspNetCore.Authorization;
using Ofix.Permissions;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ofix.CarListings
{
    [Authorize(OfixPermissions.CarListings.Default)]
    public class CarListingAppService : ApplicationService, ICarListingAppService
    {
        private readonly IRepository<CarListing, Guid> _repository;

        public CarListingAppService(IRepository<CarListing, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<CarListingDto> GetAsync(Guid id)
        {
            var queryable = await _repository.WithDetailsAsync(
                x => x.Brand,
                x => x.Model,
                x => x.SubModel
            );

            var carListing = await AsyncExecuter.FirstOrDefaultAsync(queryable.Where(x => x.Id == id));

            if (carListing == null)
            {
                throw new Exception("Car listing not found.");
            }

            return MapToCarListingDto(carListing);
        }

        public async Task<PagedResultDto<CarListingDto>> GetListAsync(CarListingListInput input)
        {
            var queryable = await _repository.WithDetailsAsync(
                x => x.Brand,
                x => x.Model,
                x => x.SubModel
            );

            var filteredQuery = queryable
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Title!))
                .WhereIf(input.SubModelId.HasValue, x => x.SubModelId == input.SubModelId.Value)
                .WhereIf(input.ListingStatus.HasValue, x => x.ListingStatus == input.ListingStatus.Value);

            var totalCount = await AsyncExecuter.CountAsync(filteredQuery);

            var carListings = await AsyncExecuter.ToListAsync(
                filteredQuery
                    .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Title" : input.Sorting)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var items = carListings.Select(MapToCarListingDto).ToList();

            return new PagedResultDto<CarListingDto>(totalCount, items);
        }

        [Authorize(OfixPermissions.CarListings.Create)]
        public async Task<CarListingDto> CreateAsync(CreateUpdateCarListingDto input)
        {
            var carListing = new CarListing
            {
                BrandId = input.BrandId,
                ModelId = input.ModelId,
                SubModelId = input.SubModelId!.Value,
                Title = input.Title,
                Price = input.Price,
                Year = input.Year,
                Mileage = input.Mileage,
                ListingStatus = input.ListingStatus!.Value,
                Description = input.Description,
                Transmission = input.Transmission!.Value,
                FuelType = input.FuelType!.Value,
                BodyShape = input.BodyShape!.Value
            };

            await _repository.InsertAsync(carListing, autoSave: true);

            var createdQuery = await _repository.WithDetailsAsync(
                x => x.Brand,
                x => x.Model,
                x => x.SubModel
            );

            var createdEntity = await AsyncExecuter.FirstOrDefaultAsync(createdQuery.Where(x => x.Id == carListing.Id));

            return MapToCarListingDto(createdEntity!);
        }

        [Authorize(OfixPermissions.CarListings.Edit)]
        public async Task<CarListingDto> UpdateAsync(Guid id, CreateUpdateCarListingDto input)
        {
            var carListing = await _repository.GetAsync(id);

            carListing.BrandId = input.BrandId;
            carListing.ModelId = input.ModelId;
            carListing.SubModelId = input.SubModelId!.Value;
            carListing.Title = input.Title;
            carListing.Price = input.Price;
            carListing.Year = input.Year;
            carListing.Mileage = input.Mileage;
            carListing.ListingStatus = input.ListingStatus!.Value;
            carListing.Description = input.Description;
            carListing.Transmission = input.Transmission!.Value;
            carListing.FuelType = input.FuelType!.Value;
            carListing.BodyShape = input.BodyShape!.Value;

            await _repository.UpdateAsync(carListing, autoSave: true);

            var updatedQuery = await _repository.WithDetailsAsync(
                x => x.Brand,
                x => x.Model,
                x => x.SubModel
            );

            var updatedEntity = await AsyncExecuter.FirstOrDefaultAsync(updatedQuery.Where(x => x.Id == id));

            return MapToCarListingDto(updatedEntity!);
        }

        [Authorize(OfixPermissions.CarListings.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        private CarListingDto MapToCarListingDto(CarListing carListing)
        {
            return new CarListingDto
            {
                Id = carListing.Id,
                Title = carListing.Title,
                BrandId = carListing.BrandId,
                BrandName = carListing.Brand?.Name,
                ModelId = carListing.ModelId,
                ModelName = carListing.Model?.Name,
                SubModelId = carListing.SubModelId,
                SubModelName = carListing.SubModel?.Name,
                Price = carListing.Price,
                Year = carListing.Year,
                Mileage = carListing.Mileage,
                ListingStatus = carListing.ListingStatus,
                Transmission = carListing.Transmission,
                FuelType = carListing.FuelType,
                BodyShape = carListing.BodyShape,
                CreationTime = carListing.CreationTime
            };
        }
    }
}