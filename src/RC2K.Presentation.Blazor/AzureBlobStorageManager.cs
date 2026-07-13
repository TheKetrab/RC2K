using Azure.Storage.Blobs;

namespace RC2K.Presentation.Blazor;

public class AzureBlobStorageManager : IStorageManager
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _blobContainerClient;

    public AzureBlobStorageManager(string connectionString)
    {
        _blobServiceClient = new(connectionString);
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient("proofs");
    }

    public async Task<string> Upload(string userName, int stageId, string fileName, Stream file)
    {
        string blobName = string.Join("-", [userName, stageId, fileName]);
        BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(file, overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }
}
