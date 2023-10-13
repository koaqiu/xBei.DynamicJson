using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using net.xBei.DynamicJson.Converters;

namespace net.xBei.DynamicJson {
    /// <summary>
    /// 动态Json，可以动态解析Json字符串，也可以动态生成Json字符串，依赖“System.Text.Json”。
    /// 不要直接使用类，而是使用派生类。
    /// </summary>
    [JsonConverter(typeof(DynamicJsonConverter))]
    public class DynamicJson {
        /// <summary>
        /// 
        /// </summary>
        protected System.Text.Json.Nodes.JsonNode? Doc { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public DynamicJson(System.Text.Json.Nodes.JsonNode? doc) {
            this.Doc = doc;
        }
        /// <summary>
        /// 读取字符串属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string? GetString(string name) => Doc?[name]?.GetValue<string>();
        /// <summary>
        /// 写入字符串属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetString(string name, string? value) => SetValue(name, value);
        /// <summary>
        /// 读取整型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected int? GetInt(string name) //=> Doc?[name]?.GetValue<int>();
            => TryGetValue(Doc?[name], s => int.TryParse(s, out var v) ? v : default);
        /// <summary>
        /// 写入整型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetInt(string name, int? value) => SetValue(name, value);
        /// <summary>
        /// 读取高精度浮点数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected decimal? GetDecimal(string name)
            => TryGetValue(Doc?[name], s => decimal.TryParse(s, out var v) ? v : default);
        /// <summary>
        /// 写入高精度浮点数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetDecimal(string name, decimal? value) => SetValue(name, value);
        /// <summary>
        /// 读取布尔值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected bool? GetBoolean(string name) => Doc?[name]?.GetValue<bool>();
        /// <summary>
        /// 写入布尔值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetBoolean(string name, bool? value) => SetValue(name, value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected DateTime? GetDateTime(string name) => Doc?[name]?.GetValue<DateTime>().ToLocalTime();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetDateTime(string name, DateTime? value) => SetValue(name, value);
        /// <summary>
        /// 读取列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<System.Text.Json.Nodes.JsonNode>? GetList(string name) {
            var node = Doc?[name];
            return node != null && node is System.Text.Json.Nodes.JsonArray array
                ? array.Where(x => x != null)
                            .Cast<System.Text.Json.Nodes.JsonNode>()
                : null;
        }
        /// <summary>
        /// 写入列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="list"></param>
        protected void SetList(string name, IEnumerable<System.Text.Json.Nodes.JsonNode>? list)
            => SetList(name, new System.Text.Json.Nodes.JsonArray(list == null ? Array.Empty<System.Text.Json.Nodes.JsonNode>() : list.ToArray()));
        /// <summary>
        /// 写入列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="list"></param>
        protected void SetList(string name, params System.Text.Json.Nodes.JsonNode[] list)
            => SetList(name, new System.Text.Json.Nodes.JsonArray(list));
        /// <summary>
        /// 写入列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="list"></param>
        protected void SetList(string name, System.Text.Json.Nodes.JsonArray? list) {
            Doc ??= new System.Text.Json.Nodes.JsonObject();
            Doc[name] = list;
        }
        /// <summary>
        /// 写入属性值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetValue(string name, System.Text.Json.Nodes.JsonNode? value) {
            Doc ??= new System.Text.Json.Nodes.JsonObject();
            Doc[name] = value;
        }
        /// <summary>
        /// 将当前实例转换为 JSON 格式的字符串。
        /// </summary>
        /// <param name="WriteIndented"></param>
        /// <returns></returns>
        public string? ToJson(bool WriteIndented = true) {
            return Doc?.ToJsonString(new JsonSerializerOptions() {
                WriteIndented = WriteIndented,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
        /// <summary>
        /// 将当前实例转换为 JSON 格式的字符串。
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public string? ToJson(JsonSerializerOptions options) {
            return Doc?.ToJsonString(options);
        }
        /// <summary>
        /// 解析Json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseJson(string json, [NotNullWhen(true)] out System.Text.Json.Nodes.JsonNode? result) {
            result = null;
            if (string.IsNullOrWhiteSpace(json)) return false;
            try {
                result = System.Text.Json.Nodes.JsonNode.Parse(json,
                    new System.Text.Json.Nodes.JsonNodeOptions {
                        PropertyNameCaseInsensitive = true,
                    });
            } catch {
            }
            return result != null;
        }
        internal static DynamicJson? TryCreate(string? json) {
            if (string.IsNullOrWhiteSpace(json)) return default;
            return TryParseJson(json, out var node) ? new DynamicJson(node) : default;
        }
        private static T? TryGetValue<T>(System.Text.Json.Nodes.JsonNode? node, Func<string, T> tryParse)
            => TryGetValue<T>(node?.AsValue(), tryParse);
        private static T? TryGetValue<T>(System.Text.Json.Nodes.JsonValue? node, Func<string, T> tryParse) {
            return node?.TryGetValue<T>(out var v) == true
                        ? v
                        : node?.TryGetValue<string>(out var s) == true
                            ? tryParse.Invoke(s)
                            : default;
        }

        internal void InitByDoc(JsonNode doc) {
            this.Doc = doc;
        }
    }
}