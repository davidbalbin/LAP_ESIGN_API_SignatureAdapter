namespace LapDrive.SignatureAdapter.Data.Clients.Interfaces;

/// <summary>
/// Client for SharePoint operations
/// </summary>
public interface ISharePointClient
{
    /// <summary>
    /// Gets the contents of a file from SharePoint
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="libraryName">The document library name</param>
    /// <param name="itemId">The item ID</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The file contents as a byte array</returns>
    Task<byte[]> GetFileContentsAsync(string webUrl, string libraryName, string itemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the contents of a folder as a zip file
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="libraryName">The document library name</param>
    /// <param name="folderId">The folder ID</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The folder contents as a zip file byte array</returns>
    Task<byte[]> GetFolderContentsAsZipAsync(string webUrl, string libraryName, string folderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates metadata for a SharePoint item
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="libraryName">The document library name</param>
    /// <param name="itemId">The item ID</param>
    /// <param name="metadata">The metadata to update</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    Task UpdateItemMetadataAsync(string webUrl, string libraryName, string itemId, Dictionary<string, object> metadata, CancellationToken cancellationToken = default);

    // Añadir a LapDrive.SignatureAdapter.Data/Clients/Interfaces/ISharePointClient.cs
    /// <summary>
    /// Creates a new list item in SharePoint
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="listName">The list name</param>
    /// <param name="metadata">The metadata to set on the item</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The ID of the created item</returns>
    Task<int> CreateListItemAsync(string webUrl, string listName, Dictionary<string, object> metadata, CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates a list item in the specified SharePoint list
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="listName">The name of the list</param>
    /// <param name="itemId">The ID of the item to update</param>
    /// <param name="fieldValues">Dictionary of field names and values to update</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task UpdateListItemAsync(string webUrl, string listName, int itemId, Dictionary<string, object> fieldValues, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list item by field value
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="listName">The list name</param>
    /// <param name="fieldName">The field name to search by</param>
    /// <param name="fieldValue">The field value to match</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The list item, or null if not found</returns>
    Task<SharePointListItem?> GetListItemByFieldValueAsync(
        string webUrl,
        string listName,
        string fieldName,
        string fieldValue,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Represents a SharePoint list item's data
    /// </summary>
    public class ListItemData
    {
        private readonly Dictionary<string, object> _fieldValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the value of a field as a string
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <returns>The field value as a string, or null if the field doesn't exist or is null</returns>
        public string? GetStringValue(string fieldName)
        {
            if (_fieldValues.TryGetValue(fieldName, out var value))
            {
                return value?.ToString();
            }

            return null;
        }

        /// <summary>
        /// Gets the value of a field as an integer
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <returns>The field value as an integer, or null if the field doesn't exist or can't be parsed</returns>
        public int? GetIntValue(string fieldName)
        {
            if (_fieldValues.TryGetValue(fieldName, out var value) && value != null)
            {
                if (value is int intValue)
                {
                    return intValue;
                }

                if (int.TryParse(value.ToString(), out var parsedValue))
                {
                    return parsedValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the value of a field as a DateTime
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <returns>The field value as a DateTime, or null if the field doesn't exist or can't be parsed</returns>
        public DateTime? GetDateTimeValue(string fieldName)
        {
            if (_fieldValues.TryGetValue(fieldName, out var value) && value != null)
            {
                if (value is DateTime dateTimeValue)
                {
                    return dateTimeValue;
                }

                if (DateTime.TryParse(value.ToString(), out var parsedValue))
                {
                    return parsedValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the value of a field as a boolean
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <returns>The field value as a boolean, or null if the field doesn't exist or can't be parsed</returns>
        public bool? GetBoolValue(string fieldName)
        {
            if (_fieldValues.TryGetValue(fieldName, out var value) && value != null)
            {
                if (value is bool boolValue)
                {
                    return boolValue;
                }

                if (bool.TryParse(value.ToString(), out var parsedValue))
                {
                    return parsedValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Sets a field value
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <param name="value">The field value</param>
        public void SetValue(string fieldName, object? value)
        {
            if (value == null)
            {
                _fieldValues.Remove(fieldName);
            }
            else
            {
                _fieldValues[fieldName] = value;
            }
        }

        /// <summary>
        /// Gets all field values
        /// </summary>
        /// <returns>A dictionary of field values</returns>
        public IReadOnlyDictionary<string, object> GetAllValues()
        {
            return _fieldValues;
        }
    }

    /// <summary>
    /// Represents a SharePoint list item
    /// </summary>
    public class SharePointListItem
    {
        /// <summary>
        /// Gets or sets the ID of the list item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the list item
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field values of the list item
        /// </summary>
        public Dictionary<string, object> FieldValues { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets a string value from the field values
        /// </summary>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>The string value or null if not found</returns>
        public string? GetStringValue(string fieldName)
        {
            if (FieldValues.TryGetValue(fieldName, out var value))
            {
                return value?.ToString();
            }

            return null;
        }

        /// <summary>
        /// Gets a DateTime value from the field values
        /// </summary>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>The DateTime value or null if not found or not a valid date</returns>
        public DateTime? GetDateTimeValue(string fieldName)
        {
            if (FieldValues.TryGetValue(fieldName, out var value) && value != null)
            {
                if (value is DateTime dateTime)
                {
                    return dateTime;
                }

                if (DateTime.TryParse(value.ToString(), out var parsedDateTime))
                {
                    return parsedDateTime;
                }
            }

            return null;
        }
    }
}
