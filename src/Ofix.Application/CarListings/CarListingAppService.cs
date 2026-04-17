using Microsoft.AspNetCore.Authorization;
using Ofix.Permissions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using System.Linq;
using System.Linq.Dynamic.Core;
using Volo.Abp.Linq;

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
            var carListing = await _repository.GetAsync(id);
            return ObjectMapper.Map<CarListing, CarListingDto>(carListing);
        }

        public async Task<PagedResultDto<CarListingDto>> GetListAsync(CarListingListInput input)
        {
            var queryable = await _repository.GetQueryableAsync();

            var filteredQuery = queryable
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title.Contains(input.Title!))
                .WhereIf(input.SubModelId.HasValue, x => x.SubModelId == input.SubModelId.Value)
                .WhereIf(input.ListingStatus.HasValue, x => x.ListingStatus == input.ListingStatus.Value);

            var carListings = await AsyncExecuter.ToListAsync(
                filteredQuery
                    .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Title" : input.Sorting)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var totalCount = await AsyncExecuter.CountAsync(filteredQuery);

            return new PagedResultDto<CarListingDto>(
                totalCount,
                ObjectMapper.Map<List<CarListing>, List<CarListingDto>>(carListings)
            );
        }

        [Authorize(OfixPermissions.CarListings.Create)]
        public async Task<CarListingDto> CreateAsync(CreateUpdateCarListingDto input)
        {
            var carListing = ObjectMapper.Map<CreateUpdateCarListingDto, CarListing>(input);

            await _repository.InsertAsync(carListing, autoSave: true);

            return ObjectMapper.Map<CarListing, CarListingDto>(carListing);
        }

        [Authorize(OfixPermissions.CarListings.Edit)]
        public async Task<CarListingDto> UpdateAsync(Guid id, CreateUpdateCarListingDto input)
        {
            var carListing = await _repository.GetAsync(id);

            carListing.SubModelId = input.SubModelId;
            carListing.Title = input.Title;
            carListing.Price = input.Price;
            carListing.Year = input.Year;
            carListing.Mileage = input.Mileage;
            carListing.ListingStatus = input.ListingStatus;
            carListing.Description = input.Description;
            carListing.Transmission = input.Transmission;
            carListing.FuelType = input.FuelType;
            carListing.BodyShape = input.BodyShape;

            await _repository.UpdateAsync(carListing, autoSave: true);

            return ObjectMapper.Map<CarListing, CarListingDto>(carListing);
        }

        [Authorize(OfixPermissions.CarListings.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}