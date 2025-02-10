using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UPatterns
{
    public static class JsonHelper
    {
        public static string Create(string key, string value) =>
            $"{{\"{key}\":\"{value}\"}}";

        public static string CreateFromList(Dictionary<string, string> keyValuePairs)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            foreach (var pair in keyValuePairs)
                stringBuilder.Append($"\"{pair.Key}\":\"{pair.Value}\",");

            // Remove the trailing comma
            if (keyValuePairs.Count > 0)
                stringBuilder.Length--;

            return stringBuilder.Append("}").ToString();
        }

        [System.Serializable]
        private class SerializableJsonWrapper<T>
        {
            public T Data;
        }
        public static string ToJson<T>(this T data) =>
            JsonUtility.ToJson(new SerializableJsonWrapper<T> { Data = data }, true);

        public static T FromJson<T>(this string json) =>
            JsonUtility.FromJson<SerializableJsonWrapper<T>>(json).Data;
    }
}