using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Ofix.Books;
using Ofix.Brands;

namespace Ofix.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixWebMappers : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);

    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}


[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixBrandWebMappers : MapperBase<BrandDto, CreateUpdateBrandDto>
{
    public override partial CreateUpdateBrandDto Map(BrandDto source);
    public override partial void Map(BrandDto source, CreateUpdateBrandDto destination);
}