// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using WireMock.Net.Testcontainers;

var container = new WireMockContainerBuilder().WithMappings(Helper.MappingsPath)
                                              .WithAutoRemove(true)
                                              .WithCleanUp(true)
                                              .Build();

await container.StartAsync().ConfigureAwait(false);

var publicBaseUrl = container.GetPublicUrl() + "api";

//Console.WriteLine("baseUrl = " + publicBaseUrl);

//var wireMockAdminClient = container.CreateWireMockAdminClient();

//var settings = await wireMockAdminClient.GetSettingsAsync();
//Console.WriteLine("settings = " + JsonConvert.SerializeObject(settings, Formatting.Indented));

//var mappings = await wireMockAdminClient.GetMappingsAsync();
//Console.WriteLine("mappings = " + JsonConvert.SerializeObject(mappings, Formatting.Indented));

var client = container.CreateClient();

Console.WriteLine("=================================Starting Tests=================================");

var dummyContent = new FormUrlEncodedContent([]);

var contentResponse = await client.PostAsync($"{publicBaseUrl}/exact", dummyContent).ConfigureAwait(false);

Console.WriteLine("Content Response = " + contentResponse.StatusCode);

Console.WriteLine("Content Response Content = " + await contentResponse.Content.ReadAsStringAsync());


await container.StopAsync();

Console.WriteLine("Finished Operations");

Console.ReadLine();

public static class Helper
{
    private static Lazy<string> RootProjectPath { get; } = new(() =>
    {
        var directoryInfo = new DirectoryInfo(AppContext.BaseDirectory);
        do
        {
            directoryInfo = directoryInfo.Parent!;
        } while (!directoryInfo.Name.EndsWith("WireMock.NET-Random-Helper-Issue", StringComparison.OrdinalIgnoreCase));

        return directoryInfo.FullName;
    });

    public static string MappingsPath => Path.Combine(RootProjectPath.Value, "Mappings");
}



