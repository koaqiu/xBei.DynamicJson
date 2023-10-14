using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace net.xBei.DynamicJson.Helper {
    /// <summary>
    /// 
    /// </summary>
    public static class DynamicJsonHelper {
        /// <summary>
        /// 解析JSON到类型：<typeparamref name="T"/>，必须继承自<see cref="DynamicJson"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryDeserialize<T>(this string? json, [NotNullWhen(true)] out T? result) where T : DynamicJson {
            result = default;
            if (string.IsNullOrWhiteSpace(json)) return false;
            result = DynamicJson.TryParseJson(json, out var node) ? CreateDynamicJson<T>(node) : default;
            return result != null;
        }
        /// <summary>
        /// 解析JSON到类型：<typeparamref name="T"/>，必须继承自<see cref="DynamicJson"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="awaitJson"></param>
        /// <param name="getDataAction"></param>
        /// <returns></returns>
        public static async Task<T> TryDeserializeAsync<T>(this Task<string?> awaitJson, Func<T> getDataAction) where T : DynamicJson {
            return TryDeserialize<T>(await awaitJson, out var data)
                    ? data
                    : getDataAction.Invoke();
        }
        /// <summary>
        /// 解析JSON到类型：<typeparamref name="T"/>，必须继承自<see cref="DynamicJson"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="awaitJson"></param>
        /// <param name="getDataActionAsync"></param>
        /// <returns></returns>
        public static async Task<T> TryDeserializeAsync<T>(this Task<string?> awaitJson, Func<Task<T>> getDataActionAsync) where T : DynamicJson {
            return TryDeserialize<T>(await awaitJson, out var data)
                    ? data
                    : await getDataActionAsync.Invoke();
        }

        /// <summary>
        /// 解析JSON到类型：<typeparamref name="T"/>，必须继承自<see cref="DynamicJson"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T? TryDeserialize<T>(this string? json) where T : DynamicJson {
            return TryDeserialize<T>(json, out var data) ? data : default;
        }
        /// <summary>
        /// 解析JSON到类型：<typeparamref name="T"/>，必须继承自<see cref="DynamicJson"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="awaitJson"></param>
        /// <returns></returns>
        public static async Task<T?> TryDeserializeAsync<T>(this Task<string?> awaitJson) where T : DynamicJson {
            return TryDeserialize<T>(await awaitJson);
        }
        /// <summary>
        /// 解析JSON到类型：<typeparamref name="T"/>，必须继承自<see cref="DynamicJson"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="dv"></param>
        /// <returns></returns>
        public static T TryDeserialize<T>(this string? json, T dv) where T : DynamicJson {
            return TryDeserialize<T>(json, out var data) ? data : dv;
        }
        /// <summary>
        /// 解析JSON到类型：<typeparamref name="T"/>，必须继承自<see cref="DynamicJson"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="awaitJson"></param>
        /// <param name="dv"></param>
        /// <returns></returns>
        public static async Task<T> TryDeserializeAsync<T>(this Task<string?> awaitJson, T dv) where T : DynamicJson {
            return TryDeserialize(await awaitJson, dv);
        }
        internal static T CreateDynamicJson<T>(JsonNode node) where T : DynamicJson {
            var inc = System.Activator.CreateInstance<T>();
            inc.InitByDoc(node);
            return inc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TT"></typeparam>
        /// <param name="thisData"></param>
        /// <param name="pro"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TT UpdateData<TT, T>(this TT thisData, T? pro, Action<T> action) where TT : DynamicJson {
            pro ??= Activator.CreateInstance<T>();
            action.Invoke(pro);
            return thisData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisData"></param>
        /// <param name="pro"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TT UpdateData<TT, T>(this TT thisData, T? pro, Action<TT, T> action) where TT : DynamicJson {
            pro ??= Activator.CreateInstance<T>();
            action.Invoke(thisData, pro);
            return thisData;
        }
    }
}
