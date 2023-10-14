using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace net.xBei.DynamicJson {
    /// <summary>
    /// 动态Json，可以动态解析Json字符串，也可以动态生成Json字符串，依赖“System.Text.Json”。
    /// 是动态读取数据的，所以如果Json数据的字段和定义的不兼容，不会在创建对象时报错，而是在读取时报错。
    /// 不要直接使用类，而是使用派生类。
    /// </summary>
    public abstract class DynamicJson {
        /// <summary>
        /// 
        /// </summary>
        protected JsonNode? Doc { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        protected DynamicJson(JsonNode? doc) {
            Doc = doc;
        }
        private Dictionary<string, object?> objRef = new();
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
        /// 读取长整型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected long? GetLong(string name)
            => TryGetValue(Doc?[name], s => long.TryParse(s, out var v) ? v : default);
        /// <summary>
        /// 写入长整型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetLong(string name, long? value) => SetValue(name, value);
        /// <summary>
        /// 读取单浮点数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected float? GetFloat(string name)
            => TryGetValue(Doc?[name], s => float.TryParse(s, out var v) ? v : default);
        /// <summary>
        /// 写入单浮点数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetFloat(string name, float? value) => SetValue(name, value);
        /// <summary>
        /// 读取双浮点数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected double? GetDouble(string name)
            => TryGetValue(Doc?[name], s => double.TryParse(s, out var v) ? v : default);
        /// <summary>
        /// 写入双浮点数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetDouble(string name, double? value) => SetValue(name, value);
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
        /// 读取日期时间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected DateTime? GetDateTime(string name) => Doc?[name]?.GetValue<DateTime>().ToLocalTime();
        /// <summary>
        /// 写入日期时间
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetDateTime(string name, DateTime? value) => SetValue(name, value);

        /// <summary>
        /// 读取列表，不建议直接使用
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<JsonNode>? GetList(string name) {
            var node = Doc?[name];
            return node != null && node is JsonArray array
                ? array.Where(x => x != null)
                            .Cast<JsonNode>()
                : null;
        }
        /// <summary>
        /// 读取字符串列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<string>? GetStringList(string name) {
            return GetList(name)?.Select(x => x.GetValue<string>());
        }
        /// <summary>
        /// 读取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected IEnumerable<T>? GetList<T>(string name) {
            return GetList(name)?.Select(x => x.GetValue<T>());
        }
        /// <summary>
        /// 读取列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetList<T>(string name, IEnumerable<T>? value) {
            var arr = value?.Select(x => JsonValue.Create(x)).ToArray();
            SetList(name, arr == null ? default : new JsonArray(arr));
        }
        /// <summary>
        /// 写入列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="list"></param>
        protected void SetList(string name, params JsonNode[] list)
            => SetList(name, new JsonArray(list));
        /// <summary>
        /// 写入列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="list"></param>
        protected void SetList(string name, JsonArray? list) {
            Doc ??= new JsonObject();
            Doc[name] = list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T? GetObject<T>(string name) where T : class, new() {
            if (!objRef.TryGetValue(name, out var obj)) {
                obj = Doc?[name]?.Deserialize<T>();
                if (obj != null) {
                    objRef[name] = obj;
                }
            }
            return (T?)obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetObject<T>(string name, T? value) where T : class, new() {
            Doc ??= new JsonObject();
            Doc[name] = JsonNode.Parse(JsonSerializer.Serialize(value));
            objRef[name] = value;
        }
        /// <summary>
        /// 写入属性值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetValue(string name, JsonNode? value) {
            Doc ??= new JsonObject();
            Doc[name] = value;
        }
        /// <summary>
        /// 将当前实例转换为 JSON 格式的字符串。
        /// </summary>
        /// <param name="WriteIndented"></param>
        /// <returns></returns>
        public string? ToJson(bool WriteIndented = true) 
            => ToJson(new JsonSerializerOptions() {
                WriteIndented = WriteIndented,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        /// <summary>
        /// 将当前实例转换为 JSON 格式的字符串。
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public string? ToJson(JsonSerializerOptions options) {
            Doc ??= new JsonObject();
            foreach (var item in objRef) {
                Doc[item.Key] = JsonNode.Parse(JsonSerializer.Serialize(item.Value));
            }
            return Doc.ToJsonString(options);
        }
        /// <summary>
        /// 解析Json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseJson(string json, [NotNullWhen(true)] out JsonNode? result) {
            result = null;
            if (string.IsNullOrWhiteSpace(json)) return false;
            try {
                result = JsonNode.Parse(json,
                    new JsonNodeOptions {
                        PropertyNameCaseInsensitive = true,
                    });
            } catch (JsonException) {
                throw;
            } catch {
            }
            return result != null;
        }

        private static T? TryGetValue<T>(JsonNode? node, Func<string, T> tryParse)
            => TryGetValue(node?.AsValue(), tryParse);
        private static T? TryGetValue<T>(JsonValue? node, Func<string, T> tryParse) {
            return node?.TryGetValue<T>(out var v) == true
                        ? v
                        : node?.TryGetValue<string>(out var s) == true
                            ? tryParse.Invoke(s)
                            : default;
        }

        internal void InitByDoc(JsonNode doc) {
            Doc = doc;
        }
    }
}