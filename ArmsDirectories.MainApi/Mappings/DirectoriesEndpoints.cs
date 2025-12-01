using ArmsDirectories.Domain.Contract.Interfaces.Repositories.Base;
using ArmsDirectories.Domain.Contract.Models.Directories;
using MainApi.Mappings;

namespace MainApi.Mappings
{
    public static class DirectoriesEndpoints
    {
        extension(IEndpointRouteBuilder builder)
        {
            public void MapDirectoriesMetadata()
            {
                var group = builder.MapGroup("/directories");
                group.MapGet("", async (IRepository<long, ArmsDirectory> repository) => {
                    var users = await repository.GetManyAsync(orderBy: q => q.OrderByDescending(x => x.Id));

                    return TypedResults.Ok(users);
                });

                group.MapGet("/{id}", async (long id, IRepository<long, ArmsDirectory> repository) =>
                {
                    var user = await repository.GetOneAsync(id);

                    return user is null ? Results.NotFound() : Results.Ok(user);
                });

                group.MapPost("", async (ArmsDirectory directory, IRepository<long, ArmsDirectory> repository, IUnitOfWork unitOfWork) =>
                {
                    var createdObject = await repository.CreateAsync(directory);
                    await unitOfWork.SaveChangesAsync();

                    return TypedResults.Created($"/directories/{createdObject!.Id}", createdObject);
                });

                group.MapPut("", async (ArmsDirectory directory, IRepository<long, ArmsDirectory> repository, IUnitOfWork unitOfWork) =>
                {
                    var updatedEntity = await repository.UpdateAsync(directory);
                    await unitOfWork.SaveChangesAsync();

                    return TypedResults.Ok(updatedEntity);
                });

                group.MapDelete("/{id}", async (long id, IRepository<long, ArmsDirectory> repository, IUnitOfWork unitOfWork) =>
                {
                    var entity = await repository.GetOneAsync(id);
                    if (entity is null)
                    {
                        return Results.NotFound();
                    }

                    await repository.DeleteAsync(entity);
                    await unitOfWork.SaveChangesAsync();

                    return Results.NoContent();
                });
            }

            public void MapDirectoriesData()
            {

            }
        }
    }
}
