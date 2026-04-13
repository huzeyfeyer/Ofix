using Ofix.Books;
using Ofix.Brands;
using Ofix.Models;
using Ofix.SubModels;
using Ofix.FeatureCategories;
using Ofix.Features;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Ofix;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixBookToBookDtoMapper : MapperBase<Book, BookDto>
{
    public override partial BookDto Map(Book source);

    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixCreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
{
    public override partial Book Map(CreateUpdateBookDto source);

    public override partial void Map(CreateUpdateBookDto source, Book destination);
}

/* Mapper classes for Brand entities */


[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixBrandToBrandDtoMapper : MapperBase<Brand, BrandDto>
{
    public override partial BrandDto Map(Brand source);

    public override partial void Map(Brand source, BrandDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixCreateUpdateBrandDtoToBrandMapper : MapperBase<CreateUpdateBrandDto, Brand>
{
    public override partial Brand Map(CreateUpdateBrandDto source);

    public override partial void Map(CreateUpdateBrandDto source, Brand destination);
}


/* Mapper classes for Model entities */

/* Mapper classes for Model entities */

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixModelToModelDtoMapper : MapperBase<Model, ModelDto>
{
    public override partial ModelDto Map(Model source);

    public override partial void Map(Model source, ModelDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixCreateUpdateModelDtoToModelMapper : MapperBase<CreateUpdateModelDto, Model>
{
    public override partial Model Map(CreateUpdateModelDto source);

    public override partial void Map(CreateUpdateModelDto source, Model destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixModelDtoToCreateUpdateModelDtoMapper : MapperBase<ModelDto, CreateUpdateModelDto>
{
    public override partial CreateUpdateModelDto Map(ModelDto source);

    public override partial void Map(ModelDto source, CreateUpdateModelDto destination);
}


/* Mapper classes for SubModel entities */

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixSubModelToSubModelDtoMapper : MapperBase<SubModel, SubModelDto>
{
    public override partial SubModelDto Map(SubModel source);
    public override partial void Map(SubModel source, SubModelDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixCreateUpdateSubModelDtoToSubModelMapper : MapperBase<CreateUpdateSubModelDto, SubModel>
{
    public override partial SubModel Map(CreateUpdateSubModelDto source);
    public override partial void Map(CreateUpdateSubModelDto source, SubModel destination);
}


/* Mapper classes for FeatureCategory entities */

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixFeatureCategoryToFeatureCategoryDtoMapper : MapperBase<FeatureCategory, FeatureCategoryDto>
{
    public override partial FeatureCategoryDto Map(FeatureCategory source);
    public override partial void Map(FeatureCategory source, FeatureCategoryDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixCreateUpdateFeatureCategoryDtoToFeatureCategoryMapper : MapperBase<CreateUpdateFeatureCategoryDto, FeatureCategory>
{
    public override partial FeatureCategory Map(CreateUpdateFeatureCategoryDto source);
    public override partial void Map(CreateUpdateFeatureCategoryDto source, FeatureCategory destination);
}


/* Mapper classes for Feature entities */
[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixFeatureToFeatureDtoMapper : MapperBase<Feature, FeatureDto>
{
    public override partial FeatureDto Map(Feature source);
    public override partial void Map(Feature source, FeatureDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixCreateUpdateFeatureDtoToFeatureMapper : MapperBase<CreateUpdateFeatureDto, Feature>
{
    public override partial Feature Map(CreateUpdateFeatureDto source);
    public override partial void Map(CreateUpdateFeatureDto source, Feature destination);
}