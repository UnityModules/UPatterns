using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UPatterns
{
    public static class UHTTP 
    {
        public record UHTTPConfig(string BaseURL, KeyValuePair<string, string>[] DefaultHeaders, bool UseBearerPrefixAuthHeader, bool Logger);
        private static UHTTPConfig Config { get; set; }
        public static void SetConfig(UHTTPConfig config) =>
            Config = config;

        private static Action onTokenExpired { get; set; }
        private static void OnTokenExpired(Action action) =>
            onTokenExpired = action;

        private static Action<bool> LoadingAction;
        public static void SetLoading(Action<bool> action) =>
            LoadingAction = action;

        private static string token = null;
        public static void SetToken(string token) =>
            UHTTP.token = token;

        private static Action<UnityWebRequest> OnError = req => Debug.LogError("ERROR: =>>> "+$"{req.url}\n"+req.error+"\n"+req.downloadHandler.text);
        public static void SetOnError(Action<UnityWebRequest> action) =>
            OnError = action;

        public static UnityWebRequest CreateRequest(string appendUrl, string method, string body = null, List<KeyValuePair<string, string>> headers = default)
        {
            UnityWebRequest req = new UnityWebRequest(Config.BaseURL + appendUrl, method);

            req.downloadHandler = new DownloadHandlerBuffer();
            if(body != null)
                req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));

            foreach (var header in Config.DefaultHeaders)
                req.SetRequestHeader(header.Key, header.Value);

            if(headers != null && headers.Count > 0)
                foreach (var header in headers)
                    req.SetRequestHeader(header.Key, header.Value);

            return req;
        }

        public static void AddToken(this UnityWebRequest request) 
            => request.SetRequestHeader("Authorization",Config.UseBearerPrefixAuthHeader ? $"Bearer {token}" : token);

        public static void Send(this UnityWebRequest request, Action onComplete = null,bool addTokenIfExist = true, bool haveLoading = false)
        {
            if(haveLoading)
                LoadingAction?.Invoke(true);

            if(addTokenIfExist && !string.IsNullOrEmpty(token))
                request.AddToken();

            request.SendWebRequest().completed += Response;

            void Response(AsyncOperation ao)
            {
                if(haveLoading) LoadingAction?.Invoke(false);

                if(request.responseCode == 401 && addTokenIfExist && token != null && onTokenExpired != null)
                {
                    token = null;
                    onTokenExpired();
                    return;
                }

                if (request.result != UnityWebRequest.Result.Success)
                    OnError?.Invoke(request);

                onComplete?.Invoke();
            }
        }

        public static T GetData<T>(this UnityWebRequest request) where T : class
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                #if UNITY_EDITOR
                Debug.Log($"[UHTTP:Error]\n[REQUEST:{request.url}]\n Error:{request.error}");
                #endif
                return null;
            }

            #if UNITY_EDITOR
            Debug.Log($"[UHTTP:Response]\n[REQUEST:{request.url}]\n Data:{request.downloadHandler.text}");
            #endif

            return JsonUtility.FromJson<T>(request.downloadHandler.text);
            //return JsonConvert.DeserializeObject<T>(request.downloadHandler.text); // Newton soft
        } 
    }
}