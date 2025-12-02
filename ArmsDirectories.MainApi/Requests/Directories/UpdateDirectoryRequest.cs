namespace MainApi.Requests.Directories;

internal sealed record UpdateDirectoryRequest(long Id, string Name, string Description, string SystemName);

internal static partial class DirectoryMappings
{
    extension(UpdateDirectoryRequest request)
    {
    }
}