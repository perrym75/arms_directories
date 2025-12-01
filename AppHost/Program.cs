using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var k8s = builder.AddKubernetesEnvironment("k8s")
       .WithProperties(k8s =>
       {
           k8s.HelmChartName = "arms-directories-app";
       });

var mainDb = builder.AddPostgres("arms-db-server")
    .WithDataVolume()
    .AddDatabase("arms-directories");


var mainApi = builder.AddProject<ArmsDirectories_MainApi>("main-api")
    .WithReference(mainDb)
    .WaitFor(mainDb);

builder.Build().Run();