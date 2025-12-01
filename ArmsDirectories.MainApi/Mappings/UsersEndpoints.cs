using ArmsDirectories.Domain.Contract.Interfaces.Repositories.Base;
using ArmsDirectories.Domain.Contract.Interfaces.Repositories.Users;
using ArmsDirectories.Domain.Contract.Models.Base;
using ArmsDirectories.Domain.Contract.Models.Users;

namespace MainApi.Mappings
{
    public static class UsersEndpoints
    {
        extension(IEndpointRouteBuilder builder)
        {
            public void MapUsers()
            {
                var group = builder.MapGroup("/users");

                group.MapGet("", async (IUserRepository userRepository) =>
                {
                    var users = await userRepository.GetManyAsync(orderBy: q => q.OrderByDescending(x => x.Id));

                    return TypedResults.Ok(users);
                });

                group.MapGet("/{id}", async (EntityId<User> id, IUserRepository userRepository) =>
                {
                    var user = await userRepository.GetOneAsync(id);

                    return user is null ? Results.NotFound() : Results.Ok(user);
                });

                group.MapPost("", async (User user, IUserRepository userRepository, IUnitOfWork unitOfWork) =>
                {
                    var createdUser = await userRepository.CreateAsync(user);
                    await unitOfWork.SaveChangesAsync();

                    return TypedResults.Created($"/users/{createdUser!.Id}", createdUser);
                });

                group.MapPut("", async (User user, IUserRepository userRepository, IUnitOfWork unitOfWork) =>
                {
                    var createdUser = await userRepository.UpdateAsync(user);
                    await unitOfWork.SaveChangesAsync();

                    return TypedResults.Ok(createdUser);
                });

                group.MapDelete("/{id}", async (EntityId<User> id, IUserRepository userRepository, IUnitOfWork unitOfWork) =>
                {
                    var user = await userRepository.GetOneAsync(id);
                    if (user is null)
                    {
                        return Results.NotFound();
                    }

                    await userRepository.DeleteAsync(user);
                    await unitOfWork.SaveChangesAsync();

                    return TypedResults.Ok(user);
                });
            }
        }
    }
}
