namespace MainApi.Mappings;

public static class Endpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapEndpoints()
        {
            builder.MapUsers();
            builder.MapDirectoriesMetadata();
            builder.MapDirectoriesData();
        }
    }
}
