using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Ofix.Books;

namespace Ofix.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OfixWebMappers : MapperBase<BookDto, CreateUpdateBookDto>
{
    public override partial CreateUpdateBookDto Map(BookDto source);

    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
}
