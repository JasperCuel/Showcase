using System;

namespace Cue.Networking
{
    /// <summary>
    /// Request to use in <seealso cref="NetworkManager.SendUnityWebRequest(WebRequestMethod, IUnityWebRequest)"/>
    /// </summary>
    public class GetRequest : IUnityWebRequest
    {
        /// <param name="requestKey">The key used to get the url value from <seealso cref="BackendConfig.GetUrlByKey(string)"/></param>
        /// <param name="successHandler">Method to call when request receives an Ok (2xx). Requires parameter value for json response</param>
        /// <param name="errorHandler">Method to call when request receives any error response. Requires parameter value for json response</param>
        public GetRequest(string requestKey, Action<string> successHandler, Action<string> errorHandler) : base(requestKey, successHandler, errorHandler)
        {
        }
    }
}