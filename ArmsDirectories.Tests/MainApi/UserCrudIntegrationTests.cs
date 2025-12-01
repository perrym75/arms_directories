using System.Net.Http.Json;
using ArmsDirectories.Tests;
using ArmsDirectories.Domain.Contract.Models.Base;
using Xunit.Abstractions;
using Xunit.Priority;
using ArmsDirectories.Domain.Contract.Models.Users;

namespace ArmsDirectories.Tests.MainApi;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class UserCrudIntegrationTests(AspireFixture fixture, ITestOutputHelper output) : IClassFixture<AspireFixture>
{
    private readonly User _newUser = new()
    {
        Id = new EntityId<User>("01942e33-6284-7c02-8f16-6f6d8bff0532"),
        Name = "John",
        Surname = "Doe",
        Email = "john.doe@gmail.com",
    };
    
    [Fact, Priority(1)]
    public async Task CreateUserReturnsCratedStatusCode()
    {
        // Arrange
        await fixture.MainApiHttpClient.DeleteAsync($"/users/{_newUser.Id}");

        // Act
        var response = await fixture.MainApiHttpClient.PostAsJsonAsync("/users", _newUser);
        output.WriteLine(await response.Content.ReadAsStringAsync());
        var user = await response.Content.ReadFromJsonAsync<User>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(user, _newUser);
    }

    [Fact, Priority(2)]
    public async Task GetUserReturnsValidValue()
    {
        // Arrange
    
        // Act
        var user = await fixture.MainApiHttpClient.GetFromJsonAsync<User>($"/users/{_newUser.Id}");
        output.WriteLine(user?.ToString());

        // Assert
        Assert.NotNull(user);
        Assert.Equal(user, _newUser);
    }
    
    [Fact, Priority(3)]
    public async Task DeleteUserReturnsOkStatusCode()
    {
        // Arrange

        // Act
        var response = await fixture.MainApiHttpClient.DeleteAsync($"/users/{_newUser.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
