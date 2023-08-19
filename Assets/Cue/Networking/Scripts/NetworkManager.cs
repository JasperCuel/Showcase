using Cue.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Cue.Networking
{
    /// <summary>
    /// Handles network requests in coroutines event-based
    /// </summary>
    public class NetworkManager : MonoSingleton<NetworkManager>
    {
        [SerializeField]
        private BackendConfig backendConfig;

        /// <summary>
        /// Sets the authentication token used in all future web requests
        /// </summary>
        /// <param name="token">The authentication token</param>
        public void SetAuthToken(string token) => BackendConfig.SetAuthToken(token);

        /// <summary>
        /// Sends a unity web request in a coroutine
        /// </summary>
        /// <param name="method">Request method (HTTP request methods)</param>
        /// <param name="request">An implementation of IUnityWebRequest (GetRequest, PostRequest, DeleteRequest, PutRequest)</param>
        public void SendUnityWebRequest(WebRequestMethod method, IUnityWebRequest request)
        {
            if (!CheckPreRequirements())
                return;

            string url = backendConfig.GetUrlByKey(request.requestKey);
            if (string.IsNullOrEmpty(url))
            {
                DebugLogger.LogWarning($"No url could be found with key {request.requestKey}", DebugType.Networking);
                return;
            }

            switch (method)
            {
                case WebRequestMethod.GET:
                    StartCoroutine(SendGet((GetRequest)request, url));
                    break;
                case WebRequestMethod.POST:
                    StartCoroutine(SendPost((PostRequest)request, url));
                    break;
                case WebRequestMethod.PUT:
                    StartCoroutine(SendPut((PutRequest)request, url));
                    break;
                case WebRequestMethod.DELETE:
                    StartCoroutine(SendDelete((DeleteRequest)request, url));
                    break;
            }
        }

        private bool CheckPreRequirements()
        {
            if (backendConfig == null)
            {
                DebugLogger.LogError("No backend config could be found", DebugType.Networking);
                return false;
            }
            if (Application.internetReachability.Equals(NetworkReachability.NotReachable))
            {
                DebugLogger.LogWarning($"Internet not reachable. Cannot perform request", DebugType.Networking);
                return false;
            }
            return true;
        }

        private IEnumerator SendGet(GetRequest getRequest, string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return SendWebRequest(request, getRequest.successHandler, getRequest.errorHandler);
        }

        private IEnumerator SendPost(PostRequest postRequest, string url)
        {
            UnityWebRequest request = UnityWebRequest.Post(url, postRequest.form);
            yield return SendWebRequest(request, postRequest.successHandler, postRequest.errorHandler);
        }

        private IEnumerator SendPut(PutRequest putRequest, string url)
        {
            UnityWebRequest request = UnityWebRequest.Post(url, putRequest.form);
            request.method = "PUT";
            yield return SendWebRequest(request, putRequest.successHandler, putRequest.errorHandler);
        }

        private IEnumerator SendDelete(DeleteRequest deleteRequest, string url)
        {
            UnityWebRequest request = UnityWebRequest.Delete(url);
            yield return SendWebRequest(request, deleteRequest.successHandler, deleteRequest.errorHandler);
        }

        private IEnumerator SendWebRequest(UnityWebRequest request, Action<string> successHandler = null, Action<string> errorHandler = null)
        {
            if (!string.IsNullOrEmpty(BackendConfig.AuthToken))
                request.SetRequestHeader("Authorization", BackendConfig.AuthToken);

            DebugLogger.Log($"Sent request to: {request.url}", DebugType.Networking);
            yield return request.SendWebRequest();

            string jsonResponse = request.downloadHandler.text;

            if (request.result.Equals(UnityWebRequest.Result.Success))
            {
                DebugLogger.Log($"Success, Response: {jsonResponse}", DebugType.Networking);
                successHandler?.Invoke(jsonResponse);
            }
            else
            {
                DebugLogger.LogError($"{request.error}: {jsonResponse}", DebugType.Networking);
                errorHandler?.Invoke(jsonResponse);
            }
            request.Dispose();
        }
    }
}