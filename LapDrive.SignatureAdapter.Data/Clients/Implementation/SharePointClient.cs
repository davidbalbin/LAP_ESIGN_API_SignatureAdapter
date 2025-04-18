using LapDrive.SignatureAdapter.Data.Clients.Interfaces;
using LapDrive.SignatureAdapter.Data.Configuration;
using LapDrive.SignatureAdapter.Data.Factories;
using LapDrive.SignatureAdapter.Models.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SharePoint.Client;
using System.IO.Compression;
using static LapDrive.SignatureAdapter.Data.Clients.Interfaces.ISharePointClient;

namespace LapDrive.SignatureAdapter.Data.Clients.Implementation;

/// <summary>
/// SharePoint client implementation
/// </summary>
public class SharePointClient : ISharePointClient
{
    private readonly SharePointContextFactory _contextFactory;
    private readonly SharePointOptions _options;
    private readonly ILogger<SharePointClient> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SharePointClient"/> class.
    /// </summary>
    /// <param name="contextFactory">The SharePoint context factory</param>
    /// <param name="options">The SharePoint options</param>
    /// <param name="logger">The logger</param>
    public SharePointClient(
        SharePointContextFactory contextFactory,
        IOptions<SharePointOptions> options,
        ILogger<SharePointClient> logger)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<byte[]> GetFileContentsAsync(string webUrl, string libraryName, string itemId, CancellationToken cancellationToken = default)
    {
        try
        {
            using var context = _contextFactory.CreateContext(webUrl);
            var web = context.Web;
            var list = web.Lists.GetByTitle(libraryName);
            var item = list.GetItemById(int.Parse(itemId));
            
            context.Load(item, i => i.File);
            await context.ExecuteQueryAsync();
            
            if (item.File == null)
            {
                throw new DataException($"Item with ID {itemId} is not a file");
            }
            
            // Get file stream without using statement
            var fileStreamResult = item.File.OpenBinaryStream();
            await context.ExecuteQueryAsync();
            
            using var memoryStream = new MemoryStream();
            await fileStreamResult.Value.CopyToAsync(memoryStream, cancellationToken);
            
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file contents from SharePoint for item {ItemId} in {WebUrl}/{LibraryName}", itemId, webUrl, libraryName);
            throw new DataException($"Error retrieving file contents: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<byte[]> GetFolderContentsAsZipAsync(string webUrl, string libraryName, string folderId, CancellationToken cancellationToken = default)
    {
        try
        {
            using var context = _contextFactory.CreateContext(webUrl);
            var web = context.Web;
            var list = web.Lists.GetByTitle(libraryName);
            var item = list.GetItemById(int.Parse(folderId));
            
            context.Load(item, i => i.Folder);
            await context.ExecuteQueryAsync();
            
            if (item.Folder == null)
            {
                throw new DataException($"Item with ID {folderId} is not a folder");
            }
            
            context.Load(item.Folder, f => f.Files);
            await context.ExecuteQueryAsync();
            
            using var zipMemoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in item.Folder.Files)
                {
                    context.Load(file, f => f.Name);
                    await context.ExecuteQueryAsync();
                    
                    if (file.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        var fileEntry = zipArchive.CreateEntry(file.Name, CompressionLevel.Optimal);
                        using var entryStream = fileEntry.Open();
                        
                        // Get file stream without using statement
                        var fileStreamResult = file.OpenBinaryStream();
                        await context.ExecuteQueryAsync();
                        
                        await fileStreamResult.Value.CopyToAsync(entryStream, cancellationToken);
                    }
                }
            }
            
            return zipMemoryStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting folder contents from SharePoint for folder {FolderId} in {WebUrl}/{LibraryName}", folderId, webUrl, libraryName);
            throw new DataException($"Error retrieving folder contents: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateItemMetadataAsync(string webUrl, string libraryName, string itemId, Dictionary<string, object> metadata, CancellationToken cancellationToken = default)
    {
        try
        {
            using var context = _contextFactory.CreateContext(webUrl);
            var web = context.Web;
            var list = web.Lists.GetByTitle(libraryName);
            var item = list.GetItemById(int.Parse(itemId));
            
            foreach (var kvp in metadata)
            {
                item[kvp.Key] = kvp.Value;
            }
            
            item.Update();
            await context.ExecuteQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating metadata for item {ItemId} in {WebUrl}/{LibraryName}", itemId, webUrl, libraryName);
            throw new DataException($"Error updating item metadata: {ex.Message}", ex);
        }
    }

    // A�adir a LapDrive.SignatureAdapter.Data/Clients/Implementation/SharePointClient.cs
    /// <inheritdoc/>
    public async Task<int> CreateListItemAsync(string webUrl, string listName, Dictionary<string, object> metadata, CancellationToken cancellationToken = default)
    {
        try
        {
            using var context = _contextFactory.CreateContext(webUrl);
            var web = context.Web;
            var list = web.Lists.GetByTitle(listName);

            var itemCreateInfo = new ListItemCreationInformation();
            var item = list.AddItem(itemCreateInfo);

            foreach (var kvp in metadata)
            {
                item[kvp.Key] = kvp.Value;
            }

            item.Update();
            await context.ExecuteQueryAsync();

            return item.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating list item in {WebUrl}/{ListName}", webUrl, listName);
            throw new DataException($"Error creating list item: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<SharePointListItem?> GetListItemByFieldValueAsync(string webUrl, string listName, string fieldName, string fieldValue, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(webUrl))
            throw new ArgumentNullException(nameof(webUrl));

        if (string.IsNullOrEmpty(listName))
            throw new ArgumentNullException(nameof(listName));

        if (string.IsNullOrEmpty(fieldName))
            throw new ArgumentNullException(nameof(fieldName));

        if (string.IsNullOrEmpty(fieldValue))
            throw new ArgumentNullException(nameof(fieldValue));

        try
        {
            _logger.LogInformation("Getting item from list {ListName} on {WebUrl} where {FieldName}={FieldValue}",
                listName, webUrl, fieldName, fieldValue);

            using var context = _contextFactory.CreateContext(webUrl);
            var list = context.Web.Lists.GetByTitle(listName);

            var camlQuery = new CamlQuery
            {
                ViewXml = $@"<View>
                           <Query>
                             <Where>
                               <Eq>
                                 <FieldRef Name='{fieldName}'/>
                                 <Value Type='Text'>{fieldValue}</Value>
                               </Eq>
                             </Where>
                           </Query>
                           <RowLimit>1</RowLimit>
                         </View>"
            };

            var items = list.GetItems(camlQuery);
            context.Load(items);
            await context.ExecuteQueryAsync();

            if (items.Count == 0)
            {
                _logger.LogInformation("No item found in list {ListName} where {FieldName}={FieldValue}",
                    listName, fieldName, fieldValue);
                return null;
            }

            var item = items[0];
            context.Load(item);
            await context.ExecuteQueryAsync();

            var result = new SharePointListItem
            {
                Id = item.Id
            };

            // Load all field values
            foreach (var fieldNameValue in item.FieldValues)
            {
                result.FieldValues[fieldNameValue.Key] = fieldNameValue.Value;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting item from list {ListName} on {WebUrl} where {FieldName}={FieldValue}",
                listName, webUrl, fieldName, fieldValue);
            throw new DataException($"Error getting SharePoint list item: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateListItemAsync(string webUrl, string listName, int itemId, Dictionary<string, object> fieldValues, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(webUrl))
            throw new ArgumentNullException(nameof(webUrl));

        if (string.IsNullOrEmpty(listName))
            throw new ArgumentNullException(nameof(listName));

        if (fieldValues == null || !fieldValues.Any())
            throw new ArgumentNullException(nameof(fieldValues), "Field values cannot be null or empty");

        try
        {
            _logger.LogInformation("Updating item {ItemId} in list {ListName} on {WebUrl}", itemId, listName, webUrl);

            using var context = _contextFactory.CreateContext(webUrl);
            var list = context.Web.Lists.GetByTitle(listName);
            var item = list.GetItemById(itemId);

            foreach (var field in fieldValues)
            {
                item[field.Key] = field.Value;
            }

            item.Update();
            await context.ExecuteQueryAsync();

            _logger.LogInformation("Successfully updated item {ItemId} in list {ListName}", itemId, listName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating item {ItemId} in list {ListName} on {WebUrl}", itemId, listName, webUrl);
            throw new DataException($"Error updating SharePoint list item: {ex.Message}", ex);
        }
    }

    // Helper method to ensure that the query executes successfully or throw a descriptive exception
    private async Task EnsureSuccessAsync(Task task)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SharePoint query execution failed");
            throw new DataException("SharePoint operation failed", ex);
        }
    }
}