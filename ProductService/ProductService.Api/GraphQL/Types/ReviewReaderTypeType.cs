using HotChocolate.Types;
using ProductService.Application.DTOs;

namespace ProductService.Api.GraphQL.Types;

/// <summary>
/// GraphQL type for ReviewReaderTypeDto.
/// </summary>
public class ReviewReaderTypeType : ObjectType<ReviewReaderTypeDto>
{
    protected override void Configure(IObjectTypeDescriptor<ReviewReaderTypeDto> descriptor)
    {
        descriptor.Field(r => r.ReaderTypeId).Type<NonNullType<IntType>>();
        descriptor.Field(r => r.ReaderType).Type<StringType>();
    }
}
