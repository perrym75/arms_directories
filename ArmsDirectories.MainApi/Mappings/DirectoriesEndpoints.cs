using ArmsDirectories.Domain.Contract.Interfaces.Repositories.Base;
using ArmsDirectories.Domain.Contract.Models.Directories;
using MainApi.Mappings;
using MainApi.Requests.Directories;

namespace MainApi.Mappings;

public static class DirectoriesEndpoints
{
    const string MetadataBasePath = "/directories";
    const string DataBasePath = "/data";

    extension(IEndpointRouteBuilder builder)
    {
        public void MapDirectoriesMetadata()
        {
            var group = builder.MapGroup(MetadataBasePath);
            group.MapGet("", async (IRepository<long, ArmsDirectory> repository) => {
                var users = await repository.GetManyAsync(orderBy: q => q.OrderByDescending(x => x.Id));

                return TypedResults.Ok(users);
            });

            group.MapGet("/{id}", async (long id, IRepository<long, ArmsDirectory> repository) =>
            {
                var entity = await repository.GetOneAsync(id);

                return entity is null ? Results.NotFound() : Results.Ok(entity);
            });

            group.MapPost("", async (CreateDirectoryRequest request, IRepository<long, ArmsDirectory> repository, IUnitOfWork unitOfWork, HttpContextAccessor accessor) =>
            {
                var entity = new ArmsDirectory()
                {
                    Name = request.Name,
                    SystemName = request.SystemName,
                    Description = request.Description,
                    CreatedBy = null!,
                    UpdatedBy = null!,
                };

                var createdEntity = await repository.CreateAsync(entity);
                await unitOfWork.SaveChangesAsync();

                return TypedResults.Created($"{MetadataBasePath}/{createdEntity!.Id}", createdEntity);
            });

            group.MapPut("/{id}", async (long id, UpdateDirectoryRequest request, IRepository<long, ArmsDirectory> repository, IUnitOfWork unitOfWork) =>
            {
                var entity = await repository.GetOneAsync(id);
                if (entity is null)
                {
                    return Results.NotFound();
                }

                entity = entity with { Name = request.Name, SystemName = request.SystemName, Description = request.Description };

                var updatedEntity = await repository.UpdateAsync(entity);
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
            var group = builder.MapGroup(DataBasePath);
            group.MapGet("/{directorySystemName}", async (string directorySystemName, IRepository<long, ArmsDirectory> directoryRepository) => {
                var directory = await directoryRepository.GetFirstAsync(d => d.SystemName == directorySystemName);

                if (directory is null)
                {
                    return Results.NotFound();
                }

                var tableName = directory.TableName;

                var users = await directoryRepository.GetManyAsync(orderBy: q => q.OrderByDescending(x => x.Id));

                return TypedResults.Ok(users);
            });

            group.MapGet("/{directorySystemName}/{id}", async (string directorySystemName, long id, IRepository<long, ArmsDirectory> repository) =>
            {
                var user = await repository.GetOneAsync(id);

                return user is null ? Results.NotFound() : Results.Ok(user);
            });

            group.MapPost("{directorySystemName}", async (string directorySystemName, CreateDirectoryRequest request,
                IRepository<long, ArmsDirectory> repository, IUnitOfWork unitOfWork, HttpContextAccessor accessor) =>
            {
                var entity = new ArmsDirectory()
                {
                    Name = request.Name,
                    SystemName = request.SystemName,
                    Description = request.Description,
                    CreatedBy = null!,
                    UpdatedBy = null!,
                };

                var createdEntity = await repository.CreateAsync(entity);
                await unitOfWork.SaveChangesAsync();

                return TypedResults.Created($"{DataBasePath}/{directorySystemName}/{createdEntity!.Id}", createdEntity);
            });

            group.MapPut("/{directorySystemName}/{id}", async (long id, UpdateDirectoryRequest request, IRepository<long, ArmsDirectory> repository, IUnitOfWork unitOfWork) =>
            {
                var entity = await repository.GetOneAsync(id);
                if (entity is null)
                {
                    return Results.NotFound();
                }

                entity = entity with { Name = request.Name, SystemName = request.SystemName, Description = request.Description };

                var updatedEntity = await repository.UpdateAsync(entity);
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
    }
}
