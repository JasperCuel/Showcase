using System.Collections.Generic;
using UnityEngine;

namespace Cue.Networking
{
    /// <summary>
    /// Config used to store the base url and a list of <seealso cref="BackendKeyValue"/> as reference
    /// </summary>
    /// <remarks>This config should only be used in <seealso cref="NetworkManager"/></remarks>
    [CreateAssetMenu(fileName = "BackendConfig", menuName = "Configs/BackendConfig")]
    public class BackendConfig : ScriptableObject
    {
        /// <summary>
        /// Authentication token used in every possible <seealso cref="NetworkManager.SendUnityWebRequest(WebRequestMethod, IUnityWebRequest)"/>
        /// </summary>
        public static string AuthToken { get; private set; }

        [SerializeField]
        private string baseUrl;
        [SerializeField]
        [Tooltip("Key is used in code to get the value. Value is added to the base url to create a request url")]
        private List<BackendKeyValue> keyValuePairs = new List<BackendKeyValue>();

        /// <summary>
        /// Gets the url by key from <seealso cref="keyValuePairs"/>
        /// </summary>
        /// <param name="key">Key to get value from</param>
        /// <returns>Full url including base url</returns>
        public string GetUrlByKey(string key)
        {
            if (string.IsNullOrEmpty(key) || keyValuePairs.Count <= 0)
                return "";

            foreach (BackendKeyValue keyValuePair in keyValuePairs)
            {
                if (keyValuePair.key.Equals(key))
                    return baseUrl + keyValuePair.value;
            }
            return "";
        }

        /// <summary>
        /// Sets the <seealso cref="AuthToken"/> variable used in <seealso cref="NetworkManager.SendUnityWebRequest(WebRequestMethod, IUnityWebRequest)"/>
        /// </summary>
        /// <param name="token">The (new) authentication token</param>
        public static void SetAuthToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return;
            AuthToken = token;
        }
    }
}