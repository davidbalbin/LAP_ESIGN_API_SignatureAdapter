namespace LapDrive.SignatureAdapter.Models.Exceptions;

/// <summary>
/// Exception thrown for data access issues
/// </summary>
public class DataException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataException"/> class.
    /// </summary>
    public DataException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DataException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DataException(string message, Exception innerException) : base(message, innerException) { }
}