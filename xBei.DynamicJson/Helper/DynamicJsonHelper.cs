using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

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
        /// <param name="json"></param>
        /// <returns></returns>
        public static T? TryDeserialize<T>(this string? json) where T : DynamicJson {
            return TryDeserialize<T>(json, out var data) ? data : default;
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
        internal static T CreateDynamicJson<T>(JsonNode node) where T : DynamicJson {
            var inc = System.Activator.CreateInstance<T>();
            inc.InitByDoc(node);
            return inc;
        }
    }
}
