using AuthorService.Application.DTO;
using AuthorService.Domain.Entities;
using HotChocolate.Types;
namespace AuthorService.Api.GraphQL
{

    public class AuthorType : ObjectType<AuthorDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AuthorDto> descriptor)
        {
            descriptor.Field(a => a.AuthorId).Type<NonNullType<IntType>>();
            descriptor.Field(a => a.AuthorName).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.AuthorDetail).Type<StringType>();
            descriptor.Field(a => a.AuthorImage).Type<StringType>();
            descriptor.Field(a => a.InvertedName).Type<BooleanType>();
            descriptor.Field(a => a.PageTitle).Type<NonNullType<StringType>>(); 


        }
    }
}
