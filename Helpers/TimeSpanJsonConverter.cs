using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlightSystem.Helpers
{
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        /// <summary>
        /// читання з .json файлу
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (TimeSpan.TryParse(reader.GetString(),
                                    out var convertedValue))
                    return convertedValue;
            }
            return default;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}