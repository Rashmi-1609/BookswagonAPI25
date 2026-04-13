using AuthorService.Data.Models;

namespace AuthorService.GraphQL
{

    public class AuthorType : ObjectType<Author>
    {
        protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
        {
            descriptor.Field(a => a.AuthorId).Type<NonNullType<IntType>>();
            descriptor.Field(a => a.Name).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.AuthorDetail).Type<StringType>();
            descriptor.Field(a => a.AuthorImage).Type<StringType>();
            descriptor.Field(a => a.FlagTopAuthor).Type<BooleanType>();
            descriptor.Field(a => a.InvertedName).Type<BooleanType>();

           
        }
    }
}
