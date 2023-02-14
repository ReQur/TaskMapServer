using System.Text.Json.Serialization;


namespace dotnetserver.Models
{
    public class ICLientLog
    {
        [JsonPropertyName("additional")]
        public object[] Additional { get; set; }
        [JsonPropertyName("columnNumber")]
        public uint ColumnNumber { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
        [JsonPropertyName("level")]
        public uint Level { get; set; }
        [JsonPropertyName("lineNumber")]
        public uint LineNumber { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

    }

    public class ClientLog : ICLientLog
    {

    }

}
