using System.Text.Json;
using System.Text.Json.Serialization;

namespace WatanaClient.API.Serialization
{
    /// <summary>
    /// Opciones de serialización JSON para la comunicación con la API
    /// </summary>
    internal static class JsonSerializationOptions
    {
        /// <summary>
        /// Opciones de serialización por defecto
        /// </summary>
        public static JsonSerializerOptions Default => new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        /// <summary>
        /// Opciones de serialización para escritura indentada (útil para depuración)
        /// </summary>
        public static JsonSerializerOptions Indented => new JsonSerializerOptions(Default)
        {
            WriteIndented = true
        };
    }
}