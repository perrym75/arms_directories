namespace MainApi.Requests.Directories;

internal sealed record CreateDirectoryRequest(string Name, string Description, string SystemName);

internal static partial class DirectoryMappings
{
    extension(CreateDirectoryRequest request)
    {
    }
}