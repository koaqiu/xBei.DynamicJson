using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using net.xBei.DynamicJson.Helper;

namespace net.xBei.DynamicJson.Converters {
    /// <summary>
    /// 解析动态Json
    /// </summary>
    public class DynamicJsonConverter : JsonConverter<DynamicJson> {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override DynamicJson? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return DynamicJson.TryCreate(reader.GetString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, DynamicJson value, JsonSerializerOptions options) {
            writer.WriteStringValue(value.ToJson(options));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicJsonConverter<T> : JsonConverter<T> where T : DynamicJson {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            return jsonDoc.RootElement.GetRawText().TryDeserialize<T>();
            //return reader.GetString()?.TryDeserialize<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
            writer.WriteRawValue(value.ToJson(
                options.Encoder == null
                ? new JsonSerializerOptions(options) {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                  }
                : options) ?? "null");
        }
    }
}
