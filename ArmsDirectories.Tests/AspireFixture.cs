using Aspire.Hosting;

namespace ArmsDirectories.Tests;

public class AspireFixture : IAsyncLifetime
{
    private DistributedApplication? _app;
    private HttpClient? _mainApiHttpClient;
    
    public HttpClient MainApiHttpClient => _mainApiHttpClient!;

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
    
        _app = await appHost.BuildAsync();
        await _app.StartAsync();

        var resourceNotificationService = _app.Services.GetRequiredService<ResourceNotificationService>();
        await resourceNotificationService.WaitForResourceAsync("main-api", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
    
        _mainApiHttpClient = _app.CreateHttpClient("main-api");
    }

    public async Task DisposeAsync()
    {
        if (_app is not null) await _app.DisposeAsync();
    }
}