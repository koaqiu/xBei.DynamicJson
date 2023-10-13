using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using net.xBei.DynamicJson.Helper;

namespace net.xBei.DynamicJson.Converters {
    /// <summary>
    /// 解析动态Json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicJsonConverter<T> : JsonConverter<T> where T : DynamicJson {
        /// <summary>
        /// 读取 JSON 并转换为类型 <typeparamref name="T"/>，必须继承<see cref="DynamicJson"/>。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var node = JsonNode.Parse(ref reader);
            if (node == null) return default;
            return DynamicJsonHelper.CreateDynamicJson<T>(node);
        }
        /// <summary>
        /// 将指定值作为 JSON 写入。
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
